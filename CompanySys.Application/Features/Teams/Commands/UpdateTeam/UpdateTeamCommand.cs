namespace CompanySys.Application.Features.Teams.Commands.UpdateTeam;

public record UpdateTeamCommand(Guid Id, string Name, string TeamLeadId) : IRequest<Result>;