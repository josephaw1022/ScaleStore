using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingDb.ScalingDb;

namespace ServiceScalingWebApi.Events;


public class CreateScalingConfigurationsForNewApplicationEvent : INotification
{
	public int ApplicationId { get; set; }

	public int ProjectId { get; set; }

	public CreateScalingConfigurationsForNewApplicationEvent(int applicationId, int projectId)
	{
		ApplicationId = applicationId;
		ProjectId = projectId;
	}
}




public class CreateScalingConfigurationsForNewApplicationEventHandler : INotificationHandler<CreateScalingConfigurationsForNewApplicationEvent>
{
	private readonly ScalingDbContext _scalingDbContext;
	private readonly ILogger<CreateScalingConfigurationsForNewApplicationEventHandler> _logger;

	public CreateScalingConfigurationsForNewApplicationEventHandler(ScalingDbContext scalingDbContext, ILogger<CreateScalingConfigurationsForNewApplicationEventHandler> logger)
	{
		_scalingDbContext = scalingDbContext;
		_logger = logger;
	}

	public async Task Handle(CreateScalingConfigurationsForNewApplicationEvent notification, CancellationToken cancellationToken)
	{
		var application = await _scalingDbContext.Applications
			.Include(a => a.Project)
			.FirstOrDefaultAsync(a => a.ApplicationID == notification.ApplicationId, cancellationToken);

		if (application == null)
		{
			_logger.LogError("Application with id {ApplicationId} not found", notification.ApplicationId);
			return;
		}

		var environments = await _scalingDbContext.Environments
			.Where(e => e.ProjectID == application.ProjectID)
			.ToListAsync(cancellationToken);

		foreach (var environment in environments)
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