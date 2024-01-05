using MediatR;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

    public class UpdateScalingConfigurationRequest : IRequest<ScalingConfigurationResponse> , IUpdateScalingConfigurationRequest
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

        public async Task<ScalingConfigurationResponse?> Handle(UpdateScalingConfigurationRequest request, CancellationToken cancellationToken)
        {
            var config = await dbContext.ScalingConfigurations.FindAsync(request.ScalingID);

            if (config is null)
            {
                return null;
            }

            if(request.ApplicationID > 0)
            {
                config.ApplicationID = request.ApplicationID;
            }


            if(request.EnvironmentID > 0)
            {
                config.EnvironmentID = request.EnvironmentID;
            }

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
