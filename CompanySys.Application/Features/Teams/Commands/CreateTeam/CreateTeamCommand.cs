namespace CompanySys.Application.Features.Teams.Commands.CreateTeam;

public record CreateTeamCommand(string Name, Guid DepartmentId, string TeamLeadId) : IRequest<Result<Guid>>;