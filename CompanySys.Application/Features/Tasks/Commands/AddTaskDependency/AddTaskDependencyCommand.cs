namespace CompanySys.Application.Features.Tasks.Commands.AddTaskDependency;

public record AddTaskDependencyCommand(Guid TaskId, Guid DependsOnTaskId) : IRequest<Result>;