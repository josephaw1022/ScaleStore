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
public class EnvironmentController : ControllerBase
{


	private readonly IMediator _mediator;


	public EnvironmentController(IMediator mediator)
	{
		_mediator = mediator;
	}


	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetOne(int id)
	{
		var request = new GetEnvironmentRequest
		{
			EnvironmentID = id
		};

		var response = await _mediator.Send(request);

		if (response is null)
		{
			return NotFound();
		}

		return Ok(response);
	}




	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(int projectId)
	{
		var request = new GetManyEnvironmentsRequest
		{
			ProjectID = projectId
		};

		var response = await _mediator.Send(request);

		return Ok(response);
	}




	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] CreateEnvironmentDTO requestBody)
	{

		var mediatrRequest = new CreateEnvironmentRequest
		{
			EnvironmentName = requestBody.EnvironmentName,
			ProjectID = requestBody.ProjectID
		};

		var response = await _mediator.Send(mediatrRequest);

		return CreatedAtAction(nameof(GetOne), new { id = response.EnvironmentID }, response);
	}



	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update([FromBody] UpdateEnvironmentDTO requestBody)
	{

		var request = new UpdateEnvironmentRequest
		{
			EnvironmentID = requestBody.EnvironmentID,
			EnvironmentName = requestBody.EnvironmentName,
			ProjectID = requestBody.ProjectID
		};

		var response = await _mediator.Send(request);

		if (response is null)
		{
			return NotFound();
		}

		return Ok(response);
	}

}