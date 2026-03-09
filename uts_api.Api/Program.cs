using System.Text;
using System.Globalization;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using uts_api.Api.Authorization;
using uts_api.Api.Middleware;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Infrastructure.DependencyInjection;
using uts_api.Infrastructure.Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddAutoMapper(typeof(uts_api.Application.Mappings.UserMappingProfile).Assembly);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddHangfireServer(options =>
{
    options.Queues = ["default", "dead-letter"];
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<uts_api.Application.Validators.Auth.LoginRequestDtoValidator>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new BadRequestObjectResult(ApiResponse.Fail(
            AppLocalizer.Get(LocalizationKeys.ValidationFailed),
            context.ModelState.IsValid
                ? [AppLocalizer.Get(LocalizationKeys.InvalidRequest)]
                : context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x =>
                        x.Exception is not null ||
                        string.IsNullOrWhiteSpace(x.ErrorMessage) ||
                        x.ErrorMessage.StartsWith("The ", StringComparison.OrdinalIgnoreCase) ||
                        x.ErrorMessage.StartsWith("A ", StringComparison.OrdinalIgnoreCase)
                            ? AppLocalizer.Get(LocalizationKeys.InvalidRequest)
                            : x.ErrorMessage)
                    .Distinct()
                    .ToArray()));
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>()?.AllowedOrigins ?? [];
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedOrigins.Count == 0)
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            return;
        }

        policy.WithOrigins(allowedOrigins.ToArray()).AllowAnyHeader().AllowAnyMethod();
    });
});

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(ApiResponse.Fail(AppLocalizer.Get(LocalizationKeys.Unauthorized)));
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(ApiResponse.Fail(AppLocalizer.Get(LocalizationKeys.Forbidden)));
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionConstants.All.Select(x => x.Code))
    {
        options.AddPolicy(PermissionPolicy.Build(permission), policy =>
            policy.RequireClaim(ClaimConstants.Permission, permission));
    }
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UTS API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
var isEfDesignTime = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_EF_DESIGNTIME"),
    "true",
    StringComparison.OrdinalIgnoreCase);

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = [60, 300, 900],
    LogEvents = true,
    OnAttemptsExceeded = AttemptsExceededAction.Fail
});
GlobalJobFilters.Filters.Add(app.Services.GetRequiredService<HangfireJobStateFilter>());

var supportedCultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new("tr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthorizationFilter()]
});
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    if (!isEfDesignTime)
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await initializer.InitializeAsync();
    }
}

if (!isEfDesignTime)
{
    RecurringJob.AddOrUpdate<IHangfireHeartbeatJob>(
        "uts-hangfire-heartbeat",
        "default",
        x => x.ExecuteAsync(),
        "*/10 * * * *");

    RecurringJob.AddOrUpdate<IStockSyncJob>(
        "rii-stock-sync-job",
        "default",
        x => x.ExecuteAsync(),
        "*/30 * * * *");

    RecurringJob.AddOrUpdate<ICustomerSyncJob>(
        "rii-customer-sync-job",
        "default",
        x => x.ExecuteAsync(),
        "*/30 * * * *");
}

app.Run();
