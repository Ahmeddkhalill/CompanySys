using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Projects.Queries.GetAllProjects;

public record ProjectResponse(Guid Id, string Name, string? Description, ProjectStatus Status, string CustomerId, int TeamsCount);

public record GetProjectsQuery(ProjectStatus? Status) : IRequest<Result<List<ProjectResponse>>>;