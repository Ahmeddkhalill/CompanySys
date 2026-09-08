using CompanySys.API.Authorization;
using CompanySys.Application.Features.Projects.Commands.AssignTeamToProject;
using CompanySys.Application.Features.Projects.Commands.CreateProject;
using CompanySys.Application.Features.Projects.Commands.RemoveTeamFromProject;
using CompanySys.Application.Features.Projects.Commands.UpdateProjectStatus;
using CompanySys.Application.Features.Projects.Queries.GetAllProjects;
using CompanySys.Application.Features.Projects.Queries.GetProjectById;
using CompanySys.Domain.Enums;

namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    [HasPermission(PermissionsList.Projects.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : result.ToProblem();
    }

    [HttpGet]
    [HasPermission(PermissionsList.Projects.Read)]
    public async Task<IActionResult> GetAll([FromQuery] ProjectStatus? status, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectsQuery(status), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(PermissionsList.Projects.Read)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}/status")]
    [HasPermission(PermissionsList.Projects.Update)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateProjectStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProjectStatusCommand(id, request.Status);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:guid}/teams")]
    [HasPermission(PermissionsList.Projects.Update)]
    public async Task<IActionResult> AssignTeam(Guid id, [FromBody] AssignTeamRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignTeamToProjectCommand(id, request.TeamId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}/teams/{teamId:guid}")]
    [HasPermission(PermissionsList.Projects.Update)]
    public async Task<IActionResult> RemoveTeam(Guid id, Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTeamFromProjectCommand(id, teamId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateProjectStatusRequest(ProjectStatus Status);
public record AssignTeamRequest(Guid TeamId);