using CompanySys.API.Authorization;
using CompanySys.Application.Contracts.Departments;
using CompanySys.Application.Features.Departments.Commands.CreateDepartment;
using CompanySys.Application.Features.Departments.Commands.DeleteDepartment;
using CompanySys.Application.Features.Departments.Commands.UpdateDepartment;
using CompanySys.Application.Features.Departments.Queries.GetAllDepartments;
using CompanySys.Application.Features.Departments.Queries.GetDepartmentById;


namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    [HasPermission(PermissionsList.Departments.Create)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : result.ToProblem();
    }

    [HttpGet]
    [HasPermission(PermissionsList.Departments.Read)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDepartmentsQuery(), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(PermissionsList.Departments.Read)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(PermissionsList.Departments.Update)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentCommand(id, request.Name, request.Description);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(PermissionsList.Departments.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}