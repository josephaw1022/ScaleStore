using MediatR;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;

public class GetScalingConfigurationRequest : IRequest<ScalingConfigurationResponse> , IGetScalingConfigurationRequest
{
    public int ScalingID { get; set; }
}

public class ScalingConfigurationResponse : IGetScalingConfigurationResponse
{
    public int ScalingID { get; set; }
    public int ApplicationID { get; set; }
    public int EnvironmentID { get; set; }
    public int NumberOfInstances { get; set; }
}

public class GetScalingConfigurationRequestHandler : IRequestHandler<GetScalingConfigurationRequest, ScalingConfigurationResponse>
{
    private readonly ScalingDbContext dbContext;

    public GetScalingConfigurationRequestHandler(ScalingDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ScalingConfigurationResponse?> Handle(GetScalingConfigurationRequest request, CancellationToken cancellationToken)
    {
        var config = await dbContext.ScalingConfigurations
            .FindAsync([request.ScalingID], cancellationToken);

        if (config is null)
        {
            return null;
        }

        return new ScalingConfigurationResponse
        {
            ScalingID = config.ScalingID,
            ApplicationID = config.ApplicationID,
            EnvironmentID = config.EnvironmentID,
            NumberOfInstances = config.NumberOfInstances
        };
    }
}
