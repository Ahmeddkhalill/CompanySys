namespace CompanySys.Application.Features.Teams.Commands.RemoveTeamMember;

public record RemoveTeamMemberCommand(Guid TeamId, string EngineerId) : IRequest<Result>;