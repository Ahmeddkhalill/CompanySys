using CompanySys.API.Authorization;
using CompanySys.Application.Contracts.Tasks;
using CompanySys.Application.Features.Tasks.Commands.AddTaskDependency;
using CompanySys.Application.Features.Tasks.Commands.AssignEngineer;
using CompanySys.Application.Features.Tasks.Commands.CloseTask;
using CompanySys.Application.Features.Tasks.Commands.CreateTask;
using CompanySys.Application.Features.Tasks.Commands.RemoveEngineer;
using CompanySys.Application.Features.Tasks.Commands.RemoveTaskDependency;
using CompanySys.Application.Features.Tasks.Commands.ReviewTask;
using CompanySys.Application.Features.Tasks.Commands.SubmitTask;
using CompanySys.Application.Features.Tasks.Commands.UpdateTaskProgress;
using CompanySys.Application.Features.Tasks.Queries.GetAllTasks;
using CompanySys.Application.Features.Tasks.Queries.GetTaskById;
using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    [HasPermission(PermissionsList.Tasks.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpGet]
    [HasPermission(PermissionsList.Tasks.ReadAssigned)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? teamId, [FromQuery] Guid? projectId, [FromQuery] DomainTaskStatus? status, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTasksQuery(teamId, projectId, status), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(PermissionsList.Tasks.ReadAssigned)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("{id:guid}/engineers")]
    [HasPermission(PermissionsList.Tasks.Assign)]
    public async Task<IActionResult> AssignEngineer(Guid id, [FromBody] AssignEngineerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AssignEngineerCommand(id, request.EngineerId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}/engineers/{engineerId}")]
    [HasPermission(PermissionsList.Tasks.Assign)]
    public async Task<IActionResult> RemoveEngineer(Guid id, string engineerId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveEngineerCommand(id, engineerId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id:guid}/progress")]
    [HasPermission(PermissionsList.Tasks.UpdateProgress)]
    public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] UpdateProgressRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateTaskProgressCommand(id, request.Progress), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:guid}/submit")]
    [HasPermission(PermissionsList.Tasks.SubmitResult)]
    public async Task<IActionResult> Submit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SubmitTaskCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:guid}/review")]
    [HasPermission(PermissionsList.Tasks.ReviewSubmission)]
    public async Task<IActionResult> Review(Guid id, [FromBody] ReviewTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReviewTaskCommand(id, request.Approved), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:guid}/close")]
    [HasPermission(PermissionsList.Tasks.Complete)]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CloseTaskCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPost("{id:guid}/dependencies")]
    [HasPermission(PermissionsList.Tasks.Assign)]
    public async Task<IActionResult> AddDependency(Guid id, [FromBody] AddDependencyRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddTaskDependencyCommand(id, request.DependsOnTaskId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}/dependencies/{dependsOnTaskId:guid}")]
    [HasPermission(PermissionsList.Tasks.Assign)]
    public async Task<IActionResult> RemoveDependency(Guid id, Guid dependsOnTaskId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTaskDependencyCommand(id, dependsOnTaskId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}