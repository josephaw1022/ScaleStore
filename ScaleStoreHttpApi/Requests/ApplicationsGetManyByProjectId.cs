﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;


namespace ScaleStoreHttpApi.Requests;


public class ApplicationsGetManyRequest : IRequest<List<ApplicationsGetManyRequestResponse>>, IApplicationsGetManyRequest
{
	public ApplicationsGetManyRequest(int projectId)
	{
		ProjectId = projectId;
	}

	public int ProjectId { get; }
}


public class ApplicationsGetManyRequestResponse : IApplicationsGetManyResponse
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public int ProjectId { get; set; }
}



public class ApplicationsGetManyRequestHandler : IRequestHandler<ApplicationsGetManyRequest, List<ApplicationsGetManyRequestResponse>>
{
	private readonly IScalingDbContext _context;

	public ApplicationsGetManyRequestHandler(IScalingDbContext context)
	{
		_context = context;
	}

	public async Task<List<ApplicationsGetManyRequestResponse>> Handle(ApplicationsGetManyRequest request, CancellationToken cancellationToken)
	{
		var applications = await _context.Applications
			.AsNoTracking()
			.Where(a => a.ProjectID == request.ProjectId)
			.ToListAsync(cancellationToken);

		return applications.Select(a => new ApplicationsGetManyRequestResponse
		{
			Id = a.ApplicationID,
			Name = a.ApplicationName,
			ProjectId = a.ProjectID
		}).ToList();
	}
}