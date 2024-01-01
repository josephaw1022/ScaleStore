using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

    public class GetManyScalingConfigurationsRequest : IRequest<List<ScalingConfigurationsTableViewResponse>>, IGetManyScalingConfigurationsRequest
    {
        public int ProjectID { get; set; }

        public GetManyScalingConfigurationsRequest(int projectID)
        {
            ProjectID = projectID;
        }
    }


    public class ScalingConfigurationsTableViewResponse : IScalingConfigurationTableViewResponse
    {

        public int Id { get; set; } = default!;

        public string ApplicationName { get; set; } = null!;

        public string EnvironmentName { get; set; } = null!;


        public int NumberOfInstances { get; set; } = default!;

    }

    public class GetManyScalingConfigurationsRequestHandler : IRequestHandler<GetManyScalingConfigurationsRequest, List<ScalingConfigurationsTableViewResponse>>
    {
        private readonly ScalingDbContext dbContext;

        public GetManyScalingConfigurationsRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ScalingConfigurationsTableViewResponse>> Handle(GetManyScalingConfigurationsRequest request, CancellationToken cancellationToken)
        {
            var scalingConfigurations = await dbContext.ScalingConfigurations
             .Include(sc => sc.Application)
             .Include(sc => sc.Environment)
             .Where(sc => sc.Application.ProjectID == request.ProjectID)
             .Select(sc => new ScalingConfigurationsTableViewResponse
                {
                 Id = sc.ScalingID,
                 ApplicationName = sc.Application.ApplicationName,
                 EnvironmentName = sc.Environment.EnvironmentName,
                 NumberOfInstances = sc.NumberOfInstances
                })
             .OrderBy(sc => sc.EnvironmentName)
             .ToListAsync();


            return scalingConfigurations;
        }
    }


}
