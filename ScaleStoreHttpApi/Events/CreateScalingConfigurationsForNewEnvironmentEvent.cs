using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingDb.ScalingDb;

namespace ServiceScalingWebApi.Events;

public class CreateScalingConfigurationsForNewEnvironmentEvent : INotification
{
	public string EnvironmentName { get; set; }

	public int ProjectId { get; set; }

	public CreateScalingConfigurationsForNewEnvironmentEvent(string environmentName, int projectId)
	{
		EnvironmentName = environmentName;
		ProjectId = projectId;
	}
}



public class CreateScalingConfigurationsForNewEnvironmentHandler : INotificationHandler<CreateScalingConfigurationsForNewEnvironmentEvent>
{
	private readonly ScalingDbContext _scalingDbContext;
	private readonly ILogger<CreateScalingConfigurationsForNewEnvironmentHandler> _logger;

	public CreateScalingConfigurationsForNewEnvironmentHandler(ScalingDbContext scalingDbContext, ILogger<CreateScalingConfigurationsForNewEnvironmentHandler> logger)
	{
		_scalingDbContext = scalingDbContext;
		_logger = logger;
	}

	public async Task Handle(CreateScalingConfigurationsForNewEnvironmentEvent notification, CancellationToken cancellationToken)
	{

		var projectId = notification.ProjectId;
		var environmentId = notification.EnvironmentName;

		_logger.LogInformation("Creating scaling configurations for new environment with name {EnvironmentId} and project id {ProjectId}", notification.EnvironmentName, notification.ProjectId);

		var environment = await _scalingDbContext.Environments
			.Where(e => e.EnvironmentName == notification.EnvironmentName && e.ProjectID == notification.ProjectId)
			.FirstOrDefaultAsync(cancellationToken);

		if (environment is null)
		{
			_logger.LogError("Environment with id {EnvironmentId} not found", notification.EnvironmentName);
			return;
		}

		var applications = await _scalingDbContext.Applications
			.Where(a => a.ProjectID == environment.ProjectID)
			.ToListAsync(cancellationToken);

		foreach (var application in applications)
		{
			var scalingConfiguration = new ScalingConfiguration
			{
				ApplicationID = application.ApplicationID,
				EnvironmentID = environment.EnvironmentID,
				NumberOfInstances = 0,
			};

			await _scalingDbContext.ScalingConfigurations.AddAsync(scalingConfiguration, cancellationToken);
		}

		await _scalingDbContext.SaveChangesAsync(cancellationToken);
	}
}