namespace CompanySys.Application.Features.Teams.Queries.GetAllTeams;

public record TeamResponse(Guid Id, string Name, Guid DepartmentId, string DepartmentName, string TeamLeadId, int MembersCount);

public record GetTeamsQuery(Guid? DepartmentId) : IRequest<Result<List<TeamResponse>>>;