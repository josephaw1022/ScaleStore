using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;
using ServiceScalingWebApi.Events;
using Environment = ServiceScalingDb.ScalingDb.Environment;

namespace ScaleStoreHttpApi.Requests;
public class CreateEnvironmentRequest : IRequest<CreateEnvironmentResponse> , ICreateEnvironmentRequest
{
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }
}

public class CreateEnvironmentResponse : ICreateEnvironmentResponse
{
    public int EnvironmentID { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }
}

public class CreateEnvironmentRequestHandler : IRequestHandler<CreateEnvironmentRequest, CreateEnvironmentResponse>
{
    private readonly ScalingDbContext dbContext;
    private readonly IMediator mediator;

    public CreateEnvironmentRequestHandler(ScalingDbContext dbContext, IMediator mediator)
    {
        this.dbContext = dbContext;
        this.mediator = mediator;
    }

    public async Task<CreateEnvironmentResponse> Handle(CreateEnvironmentRequest request, CancellationToken cancellationToken)
    {
        var newEnvironment = new Environment
        {
            EnvironmentName = request.EnvironmentName,
            ProjectID = request.ProjectID
        };

        dbContext.Environments.Add(newEnvironment);
        await dbContext.SaveChangesAsync(cancellationToken);

        await mediator.Publish(new CreateScalingConfigurationsForNewEnvironmentEvent(newEnvironment.EnvironmentName, newEnvironment.ProjectID), cancellationToken);

        return new CreateEnvironmentResponse
        {
            EnvironmentID = newEnvironment.EnvironmentID,
            EnvironmentName = newEnvironment.EnvironmentName,
            ProjectID = newEnvironment.ProjectID
        };
    }
}
