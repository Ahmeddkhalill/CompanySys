using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Projects.Commands.UpdateProjectStatus;

public record UpdateProjectStatusCommand(Guid Id, ProjectStatus Status) : IRequest<Result>;