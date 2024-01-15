using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PreferenceDb.Preference;

internal sealed class PreferenceDbInitializer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PreferenceDbInitializer> _logger;
    private readonly ActivitySource _activitySource = new(ActivitySourceName);
    public const string ActivitySourceName = "PreferenceDbInitialization";

    public PreferenceDbInitializer(IServiceProvider serviceProvider, ILogger<PreferenceDbInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PreferenceDbContext>();

        await InitializeDatabaseAsync(dbContext, cancellationToken);
    }

    private async Task InitializeDatabaseAsync(PreferenceDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        using var activity = _activitySource.StartActivity("Initializing preference database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync(cancellationToken));

        // Call seeding methods here
        // Example: await SeedProjectPreferencesAsync(dbContext, cancellationToken);

        _logger.LogInformation("Preference database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }
}
