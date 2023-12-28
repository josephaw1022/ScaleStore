using MediatR;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;

public class UpdateEnvironmentRequest : IRequest<UpdateEnvironmentResponse> , IUpdateEnvironmentRequest
{
    public int EnvironmentID { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }
}

public class UpdateEnvironmentResponse : IUpdateEnvironmentResponse
{
    public bool Success { get; set; }
}

public class UpdateEnvironmentRequestHandler : IRequestHandler<UpdateEnvironmentRequest, UpdateEnvironmentResponse>
{
    private readonly ScalingDbContext dbContext;

    public UpdateEnvironmentRequestHandler(ScalingDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UpdateEnvironmentResponse> Handle(UpdateEnvironmentRequest request, CancellationToken cancellationToken)
    {
        var environment = await dbContext.Environments.FindAsync(request.EnvironmentID);

        if (environment == null) return new UpdateEnvironmentResponse { Success = false };

        environment.EnvironmentName = request.EnvironmentName;
        environment.ProjectID = request.ProjectID;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateEnvironmentResponse { Success = true };
    }
}
