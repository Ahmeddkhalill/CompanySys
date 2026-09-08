using CompanySys.API.Authorization;
using CompanySys.Application.Contracts.Teams;
using CompanySys.Application.Features.Teams.Commands.AddTeamMember;
using CompanySys.Application.Features.Teams.Commands.CreateTeam;
using CompanySys.Application.Features.Teams.Commands.DeleteTeam;
using CompanySys.Application.Features.Teams.Commands.RemoveTeamMember;
using CompanySys.Application.Features.Teams.Commands.UpdateTeam;
using CompanySys.Application.Features.Teams.Queries.GetAllTeams;
using CompanySys.Application.Features.Teams.Queries.GetTeamById;

namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    [HasPermission(PermissionsList.Teams.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpGet]
    [HasPermission(PermissionsList.Teams.Read)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? departmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamsQuery(departmentId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(PermissionsList.Teams.Read)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(PermissionsList.Teams.Update)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTeamCommand(id, request.Name, request.TeamLeadId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(PermissionsList.Teams.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteTeamCommand(id), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:guid}/members")]
    [HasPermission(PermissionsList.Teams.ManageMembers)]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddTeamMemberRequest request, CancellationToken cancellationToken)
    {
        var command = new AddTeamMemberCommand(id, request.EngineerId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}/members/{engineerId}")]
    [HasPermission(PermissionsList.Teams.ManageMembers)]
    public async Task<IActionResult> RemoveMember(Guid id, string engineerId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTeamMemberCommand(id, engineerId), cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}