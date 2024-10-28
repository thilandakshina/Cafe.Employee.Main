using Cafe.Business.Commands.Employee;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Cafe;
using Cafe.Business.Queries.Employee;
using Cafe.Business;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetEmployees([FromQuery] Guid? cafeId = null)
        {
            try
            {
                var query = new GetEmployeesQuery { CafeId = cafeId };
                var result = await _mediator.Send(query);
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.Succeed(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<EmployeeDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var query = new GetEmployeeByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                return result != null ?
                    Ok(ApiResponse<EmployeeDto>.Succeed(result)) :
                    NotFound(ApiResponse<EmployeeDto>.Fail($"Employee with ID {id} not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EmployeeDto>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(
                    nameof(GetEmployeeById),
                    new { id = result },
                    ApiResponse<string>.Succeed(result, "Employee created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<bool>.Succeed(result, "Employee updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var command = new DeleteEmployeeCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<bool>.Succeed(result, "Employee deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }
    }
}