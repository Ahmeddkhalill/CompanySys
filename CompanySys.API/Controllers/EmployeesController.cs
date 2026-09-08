using CompanySys.API.Authorization;
using CompanySys.Application.Features.Employees.Commands.CreateEmployee;
using CompanySys.Application.Features.Employees.Commands.DeleteEmployee;
using CompanySys.Application.Features.Employees.Commands.UpdateEmployee;
using CompanySys.Application.Features.Employees.Queries.GetAllEmployees;
using CompanySys.Application.Features.Employees.Queries.GetEmployeeById;
using CompanySys.Domain.Enums;

namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    [HasPermission(PermissionsList.Employees.Create)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : result.ToProblem();
    }

    [HttpGet]
    [HasPermission(PermissionsList.Employees.Read)]
    public async Task<IActionResult> GetAll([FromQuery] EmployeeRole? role, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEmployeesQuery(role), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    [HasPermission(PermissionsList.Employees.Read)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(PermissionsList.Employees.Update)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(id, request.FirstName, request.LastName, request.Role);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(PermissionsList.Employees.Delete)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateEmployeeRequest(string FirstName, string LastName, EmployeeRole Role);