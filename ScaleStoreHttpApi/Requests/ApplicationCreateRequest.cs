using MediatR;
using ServiceScalingDb.ScalingDb;
using ServiceScalingCore;

namespace ScaleStoreHttpApi.Requests;


public class CreateApplicationRequest : IRequest<CreateApplicationResponse> , IApplicationCreate
{
    public string Name { get; set; }
    public int ProjectId { get; set; }

    public CreateApplicationRequest(string name, int projectId)
    {
        Name = name;
        ProjectId = projectId;
    }
}

public class CreateApplicationResponse : IApplicationCreateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProjectId { get; set; }
}



public class CreateApplicationRequestHandler : IRequestHandler<CreateApplicationRequest, CreateApplicationResponse>
{
    private readonly ScalingDbContext _context;

    public CreateApplicationRequestHandler(ScalingDbContext context)
    {
        _context = context;
    }

    public async Task<CreateApplicationResponse> Handle(CreateApplicationRequest request, CancellationToken cancellationToken)
    {
        var newApplication = new Application
        {
            ApplicationName = request.Name,
            ProjectID = request.ProjectId
        };

        _context.Applications.Add(newApplication);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateApplicationResponse
        {
            Id = newApplication.ApplicationID,
            Name = newApplication.ApplicationName,
            ProjectId = newApplication.ProjectID
        };
    }
}
