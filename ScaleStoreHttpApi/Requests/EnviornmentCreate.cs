﻿using MediatR;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;
public class CreateEnvironmentRequest : IRequest<CreateEnvironmentResponse> , ICreateEnvironmentRequest
{
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }

}

public class CreateEnvironmentResponse : ICreateEnvironmentResponse
{
    public int EnvironmentID { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }
}

public class CreateEnvironmentRequestHandler : IRequestHandler<CreateEnvironmentRequest, CreateEnvironmentResponse>
{
    private readonly ScalingDbContext dbContext;

    public CreateEnvironmentRequestHandler(ScalingDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CreateEnvironmentResponse> Handle(CreateEnvironmentRequest request, CancellationToken cancellationToken)
    {
        var newEnvironment = new ServiceScalingDb.ScalingDb.Environment
        {
            EnvironmentName = request.EnvironmentName,
            ProjectID = request.ProjectID
        };

        dbContext.Environments.Add(newEnvironment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateEnvironmentResponse
        {
            EnvironmentID = newEnvironment.EnvironmentID,
            EnvironmentName = newEnvironment.EnvironmentName,
            ProjectID = newEnvironment.ProjectID
        };
    }
}
