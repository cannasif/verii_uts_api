using uts_api.Application.Common.Interfaces;

namespace uts_api.Infrastructure.Persistence.Seed;

public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly UtsDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseInitializer(UtsDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await DbSeeder.SeedAsync(_dbContext, _passwordHasher, cancellationToken);
    }
}
