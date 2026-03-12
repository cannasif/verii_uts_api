using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Hangfire;
using uts_api.Infrastructure.Persistence;
using uts_api.Infrastructure.Persistence.Interceptors;
using uts_api.Infrastructure.Persistence.Seed;
using uts_api.Infrastructure.Security;
using uts_api.Infrastructure.Services;

namespace uts_api.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteSaveChangesInterceptor>();
        services.Configure<HangfireMonitoringOptions>(configuration.GetSection(HangfireMonitoringOptions.SectionName));

        services.AddDbContext<UtsDbContext>((serviceProvider, options) =>
            options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(
                    serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(),
                    serviceProvider.GetRequiredService<SoftDeleteSaveChangesInterceptor>()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<UtsDbContext>());
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionDefinitionService, PermissionDefinitionService>();
        services.AddScoped<IPermissionGroupService, PermissionGroupService>();
        services.AddScoped<IUserPermissionGroupService, UserPermissionGroupService>();
        services.AddScoped<ISmtpSettingService, SmtpSettingService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IHangfireMonitoringService, HangfireMonitoringService>();
        services.AddScoped<IStockFunctionService, StockFunctionService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<ICustomerFunctionService, CustomerFunctionService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IUtsVermeListService, UtsVermeListService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IHangfireDeadLetterJob, HangfireDeadLetterJob>();
        services.AddScoped<IHangfireHeartbeatJob, HangfireHeartbeatJob>();
        services.AddScoped<IStockSyncJob, StockSyncJob>();
        services.AddScoped<ICustomerSyncJob, CustomerSyncJob>();
        services.AddSingleton<HangfireJobStateFilter>();

        return services;
    }
}
