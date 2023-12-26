using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace ServiceScalingDb.ScalingDb;

internal sealed class ScalingDbInitializer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScalingDbInitializer> _logger;
    private readonly ActivitySource _activitySource = new(ActivitySourceName);
    public const string ActivitySourceName = "ScalingDbInitialization";

    public ScalingDbInitializer(IServiceProvider serviceProvider, ILogger<ScalingDbInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ScalingDbContext>();

        await InitializeDatabaseAsync(dbContext, cancellationToken);
    }

    private async Task InitializeDatabaseAsync(ScalingDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        using var activity = _activitySource.StartActivity("Initializing scaling database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync(cancellationToken));

        await SeedProjectAsync(dbContext, cancellationToken);

        _logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }





    private async Task SeedProjectAsync(ScalingDbContext dbContext, CancellationToken cancellationToken)
    {
        // create a single project
        var project = new Project
        {
            ProjectName = "Test Project"
        };

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        project = await dbContext.Projects
            .Where(p => p.ProjectName == "Test Project")
            .FirstOrDefaultAsync(cancellationToken);

        // create two dev environments
        var devEnvironment = new Environment
        {
            EnvironmentName = "Dev",
            ProjectID = 1
        };

        var devEnvironment2 = new Environment
        {
            EnvironmentName = "Dev2",
            ProjectID = 1
        };

        await dbContext.Environments.AddRangeAsync(devEnvironment, devEnvironment2);
        await dbContext.SaveChangesAsync(cancellationToken);

        devEnvironment = await dbContext.Environments
            .Where(e => e.EnvironmentName == "Dev")
            .FirstOrDefaultAsync(cancellationToken);

        devEnvironment2 = await dbContext.Environments
            .Where(e => e.EnvironmentName == "Dev2")
            .FirstOrDefaultAsync(cancellationToken);

        // create two applications
        var application1 = new Application
        {
            ApplicationName = "Test Application 1",
            ProjectID = 1
        };

        var application2 = new Application
        {
            ApplicationName = "Test Application 2",
            ProjectID = 1
        };

        dbContext.Applications.Add(application1);
        dbContext.Applications.Add(application2);

        await dbContext.SaveChangesAsync(cancellationToken);


        application1 = await dbContext.Applications
            .Where(a => a.ApplicationName == "Test Application 1")
            .FirstOrDefaultAsync(cancellationToken);

        application2 = await dbContext.Applications
            .Where(a => a.ApplicationName == "Test Application 2")
            .FirstOrDefaultAsync(cancellationToken);




        // create two scaling configurations

        var scalingConfiguration1 = new ScalingConfiguration
        {
            ApplicationID = application1?.ApplicationID ?? 1,
            EnvironmentID = devEnvironment?.EnvironmentID ?? 1,
            NumberOfInstances = 2,
        };
        

        var scalingConfiguration2 = new ScalingConfiguration
        {
            ApplicationID = application2?.ApplicationID ?? 2,
            EnvironmentID = devEnvironment2?.EnvironmentID ?? 2,
            NumberOfInstances = 3,
        };


        dbContext.ScalingConfigurations.AddRange(scalingConfiguration1, scalingConfiguration2);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}