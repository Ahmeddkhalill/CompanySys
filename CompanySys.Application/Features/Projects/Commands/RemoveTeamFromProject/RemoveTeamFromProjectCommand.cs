namespace CompanySys.Application.Features.Projects.Commands.RemoveTeamFromProject;

public record RemoveTeamFromProjectCommand(Guid ProjectId, Guid TeamId) : IRequest<Result>;