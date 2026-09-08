namespace CompanySys.Application.Features.Tasks.Commands.RemoveTaskDependency;

public record RemoveTaskDependencyCommand(Guid TaskId, Guid DependsOnTaskId) : IRequest<Result>;