using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

    public class GetManyScalingConfigurationsRequest : IRequest<List<ScalingConfigurationResponse>>
    {
        public int ProjectID { get; set; }

        public GetManyScalingConfigurationsRequest(int projectID)
        {
            ProjectID = projectID;
        }
    }

    public class GetManyScalingConfigurationsRequestHandler : IRequestHandler<GetManyScalingConfigurationsRequest, List<ScalingConfigurationResponse>>
    {
        private readonly ScalingDbContext dbContext;

        public GetManyScalingConfigurationsRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ScalingConfigurationResponse>> Handle(GetManyScalingConfigurationsRequest request, CancellationToken cancellationToken)
        {
            var configs = await dbContext.ScalingConfigurations
                .Where(c => c.Environment.ProjectID == request.ProjectID)
                .ToListAsync(cancellationToken);

            return configs.Select(c => new ScalingConfigurationResponse
            {
                ScalingID = c.ScalingID,
                ApplicationID = c.ApplicationID,
                EnvironmentID = c.EnvironmentID,
                NumberOfInstances = c.NumberOfInstances
            }).ToList();
        }
    }


}
