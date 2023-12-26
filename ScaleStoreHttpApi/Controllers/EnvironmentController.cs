using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScaleStoreHttpApi.Requests;

namespace ScaleStoreHttpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Create([FromBody] CreateEnvironmentRequest request)
        {
            var response = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetOne), new { id = response.EnvironmentID }, response);
        }



        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateEnvironmentRequest request)
        {
            var response = await _mediator.Send(request);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

    }
}
