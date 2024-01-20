using MediatR;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

using ServiceScalingWebApi.Events;

namespace ScaleStoreHttpApi.Requests;


public class CreateApplicationRequest : IRequest<CreateApplicationResponse>, IApplicationCreate
{
	public string Name { get; set; }
	public int ProjectId { get; set; }

	public CreateApplicationRequest(string name, int projectId)
	{
		Name = name;
		ProjectId = projectId;
	}
}

public class CreateApplicationResponse : IApplicationCreateResponse
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public int ProjectId { get; set; }
}



public class CreateApplicationRequestHandler : IRequestHandler<CreateApplicationRequest, CreateApplicationResponse>
{
	private readonly IScalingDbContext _context;
	private readonly ILogger<CreateApplicationRequestHandler> _logger;
	private readonly IMediator _mediator;

	public CreateApplicationRequestHandler(IScalingDbContext context, ILogger<CreateApplicationRequestHandler> logger, IMediator mediator)
	{
		_context = context;
		_logger = logger;
		_mediator = mediator;
	}

	public async Task<CreateApplicationResponse> Handle(CreateApplicationRequest request, CancellationToken cancellationToken)
	{
		var newApplication = new Application
		{
			ApplicationName = request.Name,
			ProjectID = request.ProjectId
		};

		_logger.LogInformation($"Creating new application {newApplication.ApplicationName} for project {newApplication.ProjectID}");

		await _context.Applications.AddAsync(newApplication);
		await _context.SaveChangesAsync(cancellationToken);


		await _mediator.Publish(new CreateScalingConfigurationsForNewApplicationEvent(newApplication.ApplicationID, newApplication.ProjectID), cancellationToken);

		return new CreateApplicationResponse
		{
			Id = newApplication.ApplicationID,
			Name = newApplication.ApplicationName,
			ProjectId = newApplication.ProjectID
		};
	}
}