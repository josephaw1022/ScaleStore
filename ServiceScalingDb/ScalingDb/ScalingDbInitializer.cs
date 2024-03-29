﻿using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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
		await SeedProjectOne(dbContext, cancellationToken);
		await SeedProjectTwo(dbContext, cancellationToken);
	}



	private async Task SeedProjectOne(ScalingDbContext dbContext, CancellationToken cancellationToken)
	{

		if (await dbContext.Projects.AnyAsync(p => p.ProjectName == "PathwayPlus", cancellationToken))
		{
			return;
		}
		var project1 = new Project { ProjectName = "PathwayPlus" };
		await dbContext.Projects.AddAsync(project1);
		await dbContext.SaveChangesAsync(cancellationToken);

		var devEnvironment = new Environment { EnvironmentName = "DevSite1", Project = project1 };
		var devEnvironment2 = new Environment { EnvironmentName = "DevSite2", Project = project1 };
		var devEnvironment3 = new Environment { EnvironmentName = "DevSite3", Project = project1 };

		await dbContext.Environments.AddRangeAsync(devEnvironment, devEnvironment2, devEnvironment3);
		await dbContext.SaveChangesAsync(cancellationToken);

		var application1 = new Application { ApplicationName = "Api Gateway", Project = project1 };
		var application2 = new Application { ApplicationName = "NextJs UI/API", Project = project1 };
		await dbContext.Applications.AddRangeAsync(application1, application2);
		await dbContext.SaveChangesAsync(cancellationToken);

		var projectOneApps = new Application[] { application1, application2 };
		var projectOneEnvs = new Environment[] { devEnvironment, devEnvironment2, devEnvironment3 };

		foreach (var project in projectOneApps)
		{
			foreach (var env in projectOneEnvs)
			{
				var scalingConfig = new ScalingConfiguration
				{
					Application = project,
					Environment = env,
					NumberOfInstances = new Random().Next(6, 20),
				};
				dbContext.ScalingConfigurations.Add(scalingConfig);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}


	private async Task SeedProjectTwo(ScalingDbContext dbContext, CancellationToken cancellationToken)
	{

		if (await dbContext.Projects.AnyAsync(p => p.ProjectName == "MendMind", cancellationToken))
		{
			return;
		}

		var project = new Project { ProjectName = "MendMind" };
		await dbContext.Projects.AddAsync(project);
		await dbContext.SaveChangesAsync(cancellationToken);

		// Log the project to see if saving it made any changes to the project object 
		_logger.LogInformation("Project ID after db save: {ProjectID}", project.ProjectID);

		var devEnvironment = new Environment { EnvironmentName = "Dev", Project = project };
		var qaEnvironment = new Environment { EnvironmentName = "QA", Project = project };
		var prodEnvironment = new Environment { EnvironmentName = "Prod", Project = project };
		var devsite1Environment = new Environment { EnvironmentName = "DevSite1", Project = project };
		var devsite2Environment = new Environment { EnvironmentName = "DevSite2", Project = project };
		var devsite3Environment = new Environment { EnvironmentName = "DevSite3", Project = project };
		var devsite4Environment = new Environment { EnvironmentName = "DevSite4", Project = project };
		var devsite5Environment = new Environment { EnvironmentName = "DevSite5", Project = project };
		var devsite6Environment = new Environment { EnvironmentName = "DevSite6", Project = project };
		var devsite7Environment = new Environment { EnvironmentName = "DevSite7", Project = project };
		var devsite8Environment = new Environment { EnvironmentName = "DevSite8", Project = project };
		var devsite9Environment = new Environment { EnvironmentName = "DevSite9", Project = project };
		var devsite10Environment = new Environment { EnvironmentName = "DevSite10", Project = project };



		await dbContext.Environments.AddRangeAsync(devEnvironment, qaEnvironment, prodEnvironment, devsite1Environment, devsite2Environment, devsite3Environment, devsite4Environment, devsite5Environment, devsite6Environment, devsite7Environment, devsite8Environment, devsite9Environment, devsite10Environment);
		await dbContext.SaveChangesAsync(cancellationToken);

		var authorizationMicroservice = new Application
		{
			ApplicationName = "Authorization Microservice",
			Project = project
		};

		var dailyStatusMicroservice = new Application
		{
			ApplicationName = "Daily Status MicroService",
			Project = project
		};

		var journalingMicroservice = new Application
		{
			ApplicationName = "Journaling Microservice",
			Project = project
		};

		var moodTrackingMicroservice = new Application
		{
			ApplicationName = "Mood Tracking Microservice",
			Project = project
		};

		var excelFileGenerationMicroservice = new Application
		{
			ApplicationName = "Excel File Generation Microservice",
			Project = project
		};

		await dbContext.Applications.AddRangeAsync(authorizationMicroservice, dailyStatusMicroservice, journalingMicroservice, moodTrackingMicroservice, excelFileGenerationMicroservice);
		await dbContext.SaveChangesAsync(cancellationToken);

		var projectApps = new Application[] { authorizationMicroservice, dailyStatusMicroservice, journalingMicroservice, moodTrackingMicroservice, excelFileGenerationMicroservice };
		var projectEnvs = new Environment[] { devEnvironment, qaEnvironment, prodEnvironment, devsite1Environment, devsite2Environment, devsite3Environment, devsite4Environment, devsite5Environment, devsite6Environment, devsite7Environment, devsite8Environment, devsite9Environment, devsite10Environment };

		foreach (var app in projectApps)
		{
			foreach (var env in projectEnvs)
			{
				var scalingConfig = new ScalingConfiguration
				{
					Application = app,
					Environment = env,
					NumberOfInstances = new Random().Next(6, 20),
				};
				dbContext.ScalingConfigurations.Add(scalingConfig);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}