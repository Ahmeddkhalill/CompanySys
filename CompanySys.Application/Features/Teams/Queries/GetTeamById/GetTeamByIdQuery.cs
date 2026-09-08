namespace CompanySys.Application.Features.Teams.Queries.GetTeamById;

public record TeamDetailsResponse(
    Guid Id,
    string Name,
    Guid DepartmentId,
    string DepartmentName,
    string TeamLeadId,
    List<string> MemberIds);

public record GetTeamByIdQuery(Guid Id) : IRequest<Result<TeamDetailsResponse>>;