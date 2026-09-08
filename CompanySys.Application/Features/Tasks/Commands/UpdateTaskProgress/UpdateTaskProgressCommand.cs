namespace CompanySys.Application.Features.Tasks.Commands.UpdateTaskProgress;

public record UpdateTaskProgressCommand(Guid TaskId, int Progress) : IRequest<Result>;