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
public class ScalingConfigurationController : ControllerBase
{
	private readonly IMediator _mediator;

	public ScalingConfigurationController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		var response = await _mediator.Send(new GetScalingConfigurationRequest { ScalingID = id });
		if (response == null) return NotFound();
		return Ok(response);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll(int projectId, int applicationId = 0)
	{
		var response = await _mediator.Send(new GetManyScalingConfigurationsRequest(projectId, applicationId));
		return Ok(response);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateScalingConfigurationDTO requestBody)
	{
		var request = new CreateScalingConfigurationRequest
		{
			ApplicationID = requestBody.ApplicationID,
			EnvironmentID = requestBody.EnvironmentID,
			NumberOfInstances = requestBody.NumberOfInstances
		};


		var response = await _mediator.Send(request);
		return CreatedAtAction(nameof(Get), new { id = response.ScalingID }, response);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateScalingConfigurationDTO requestBody)
	{
		if (id != requestBody.ScalingID)
		{
			return BadRequest("ID mismatch");
		}


		var request = new UpdateScalingConfigurationRequest
		{
			ScalingID = requestBody.ScalingID,
			ApplicationID = requestBody.ApplicationID,
			EnvironmentID = requestBody.EnvironmentID,
			NumberOfInstances = requestBody.NumberOfInstances
		};

		var response = await _mediator.Send(request);
		if (response == null) return NotFound();
		return Ok(response);
	}
}