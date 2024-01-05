using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;
using System.Text.Json;

namespace ScaleStoreHttpApi.Requests
{

    public class GetManyScalingConfigurationsRequest : IRequest<List<ScalingConfigurationsTableViewResponse>>, IGetManyScalingConfigurationsRequest
    {
        public int ProjectID { get; set; }

        public int ApplicationId { get; set; }

        public GetManyScalingConfigurationsRequest(int projectID, int applicationId)
        {
            ProjectID = projectID;
            ApplicationId = applicationId;
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

        private readonly ILogger<GetManyScalingConfigurationsRequestHandler> logger;

        public GetManyScalingConfigurationsRequestHandler(ScalingDbContext dbContext, ILogger<GetManyScalingConfigurationsRequestHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<ScalingConfigurationsTableViewResponse>> Handle(GetManyScalingConfigurationsRequest request, CancellationToken cancellationToken)
        {
            // Start the query
            var query = dbContext.ScalingConfigurations
                .Include(sc => sc.Application)
                .Include(sc => sc.Environment)
                .Where(sc => sc.Application.ProjectID == request.ProjectID);

            // Conditionally add the ApplicationID filter
            if (request.ApplicationId > 0)
            {
                query = query.Where(sc => sc.Application.ApplicationID == request.ApplicationId);
            }

            // Continue building the query and execute it
            var scalingConfigurations = await query
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
