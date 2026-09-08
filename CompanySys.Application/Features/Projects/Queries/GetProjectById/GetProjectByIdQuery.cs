using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Projects.Queries.GetProjectById;

public record ProjectTeamDto(Guid Id, string Name);

public record ProjectDetailsResponse(
    Guid Id, string Name, string? Description, ProjectStatus Status, string CustomerId, List<ProjectTeamDto> Teams);

public record GetProjectByIdQuery(Guid Id) : IRequest<Result<ProjectDetailsResponse>>;