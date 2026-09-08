namespace CompanySys.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : IRequest<Result<Guid>>;