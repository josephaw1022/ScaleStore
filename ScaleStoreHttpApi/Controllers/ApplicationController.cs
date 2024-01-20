using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using ScaleStoreHttpApi.Requests;

using ServiceScalingDTO;


namespace ScaleStoreHttpApi.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[OutputCache(Duration = 15)]
public class ApplicationController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<ApplicationController> _logger;

	public ApplicationController(IMediator mediator, ILogger<ApplicationController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetOne(int id)
	{

		var getOne = await _mediator.Send(new ApplicationGetOneRequest(id));

		if (getOne is null)
		{
			return NotFound();
		}

		return Ok(getOne);
	}

	[HttpGet]
	public async Task<IActionResult> GetApplicationsByProject([FromQuery] int projectId)
	{
		var getMany = await _mediator.Send(new ApplicationsGetManyRequest(projectId));
		return Ok(getMany);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateApplicationDTO application)
	{
		_logger.LogInformation($"Creating new application {application.Name} for project {application.ProjectId}");

		var create = await _mediator.Send(new CreateApplicationRequest(application.Name, application.ProjectId));
		return CreatedAtAction(nameof(GetOne), new { ApplicationId = create.Id }, create);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateApplicationDTO application)
	{

		var update = await _mediator.Send(new UpdateApplicationRequest(id, application.Name, application.ProjectId));


		if (!update.Success)
		{
			return NotFound();
		}

		return Ok(update);

	}
}