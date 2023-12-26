using MediatR;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

    public class UpdateScalingConfigurationRequest : IRequest<ScalingConfigurationResponse>
    {
        public int ScalingID { get; set; }
        public int ApplicationID { get; set; }
        public int EnvironmentID { get; set; }
        public int NumberOfInstances { get; set; }
    }

    public class UpdateScalingConfigurationRequestHandler : IRequestHandler<UpdateScalingConfigurationRequest, ScalingConfigurationResponse>
    {
        private readonly ScalingDbContext dbContext;

        public UpdateScalingConfigurationRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ScalingConfigurationResponse> Handle(UpdateScalingConfigurationRequest request, CancellationToken cancellationToken)
        {
            var config = await dbContext.ScalingConfigurations.FindAsync(request.ScalingID);

            if (config is null)
            {
                return null;
            }

            config.ApplicationID = request.ApplicationID;
            config.EnvironmentID = request.EnvironmentID;
            config.NumberOfInstances = request.NumberOfInstances;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new ScalingConfigurationResponse
            {
                ScalingID = config.ScalingID,
                ApplicationID = config.ApplicationID,
                EnvironmentID = config.EnvironmentID,
                NumberOfInstances = config.NumberOfInstances
            };
        }
    }

}
