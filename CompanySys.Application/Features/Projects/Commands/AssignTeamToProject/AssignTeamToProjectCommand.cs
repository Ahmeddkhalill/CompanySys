namespace CompanySys.Application.Features.Projects.Commands.AssignTeamToProject;

public record AssignTeamToProjectCommand(Guid ProjectId, Guid TeamId) : IRequest<Result>;