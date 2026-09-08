namespace CompanySys.Application.Features.Teams.Commands.DeleteTeam;

public record DeleteTeamCommand(Guid Id) : IRequest<Result>;