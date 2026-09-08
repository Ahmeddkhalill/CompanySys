namespace CompanySys.Application.Features.Teams.Commands.AddTeamMember;

public record AddTeamMemberCommand(Guid TeamId, string EngineerId) : IRequest<Result>;