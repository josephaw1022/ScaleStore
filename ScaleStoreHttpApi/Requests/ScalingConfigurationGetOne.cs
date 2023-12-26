namespace ScaleStoreHttpApi.Requests
{
    using MediatR;
    using ServiceScalingDb.ScalingDb;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetScalingConfigurationRequest : IRequest<ScalingConfigurationResponse>
    {
        public int ScalingID { get; set; }
    }

    public class ScalingConfigurationResponse
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

        public async Task<ScalingConfigurationResponse> Handle(GetScalingConfigurationRequest request, CancellationToken cancellationToken)
        {
            var config = await dbContext.ScalingConfigurations
                .FindAsync(new object[] { request.ScalingID }, cancellationToken);

            if (config == null) return null;

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
