using Cafe.Business;
using Cafe.Business.Commands.Cafe;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Cafe;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CafeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CafeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CafeDto>>>> GetCafes([FromQuery] string location = null)
        {
            try
            {
                var query = new GetCafesQuery { Location = location };
                var result = await _mediator.Send(query);
                return Ok(ApiResponse<IEnumerable<CafeDto>>.Succeed(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<CafeDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCafeById(Guid id)
        {
            try
            {
                var query = new GetCafeByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                return result != null ? Ok(result) : NotFound($"Cafe with Location {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCafe([FromBody] CreateCafeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetCafeById), new { id = result }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCafe(Guid id, [FromBody] UpdateCafeCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCafe(Guid id)
        {
            try
            {
                var command = new DeleteCafeCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
