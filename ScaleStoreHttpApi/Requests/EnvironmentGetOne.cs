using MediatR;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;

public class GetEnvironmentRequest : IRequest<GetEnvironmentResponse> , IGetEnvironmentRequest
{
    public int EnvironmentID { get; set; }
}

public class GetEnvironmentResponse : IGetEnvironmentResponse
{
    public int EnvironmentID { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }
}

public class GetEnvironmentRequestHandler : IRequestHandler<GetEnvironmentRequest, GetEnvironmentResponse>
{
    private readonly IScalingDbContext dbContext;

    public GetEnvironmentRequestHandler(IScalingDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<GetEnvironmentResponse> Handle(GetEnvironmentRequest request, CancellationToken cancellationToken)
    {
        var environment = await dbContext.Environments
            .FindAsync(new object[] { request.EnvironmentID }, cancellationToken);

        if (environment == null) return null;

        return new GetEnvironmentResponse
        {
            EnvironmentID = environment.EnvironmentID,
            EnvironmentName = environment.EnvironmentName,
            ProjectID = environment.ProjectID
        };
    }
}
