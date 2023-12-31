﻿using MediatR;
using ServiceScalingDb.ScalingDb;
using ServiceScalingCore;

namespace ScaleStoreHttpApi.Requests
{
    public class CreateScalingConfigurationRequest : IRequest<ScalingConfigurationResponse> , ICreateScalingConfigurationRequest
    {
        public int ApplicationID { get; set; }
        public int EnvironmentID { get; set; }
        public int NumberOfInstances { get; set; }
    }

    public class CreateScalingConfigurationRequestHandler : IRequestHandler<CreateScalingConfigurationRequest, ScalingConfigurationResponse>
    {
        private readonly ScalingDbContext dbContext;

        public CreateScalingConfigurationRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ScalingConfigurationResponse> Handle(CreateScalingConfigurationRequest request, CancellationToken cancellationToken)
        {
            var newConfig = new ScalingConfiguration
            {
                ApplicationID = request.ApplicationID,
                EnvironmentID = request.EnvironmentID,
                NumberOfInstances = request.NumberOfInstances
            };

            dbContext.ScalingConfigurations.Add(newConfig);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ScalingConfigurationResponse
            {
                ScalingID = newConfig.ScalingID,
                ApplicationID = newConfig.ApplicationID,
                EnvironmentID = newConfig.EnvironmentID,
                NumberOfInstances = newConfig.NumberOfInstances
            };
        }
    }

}
