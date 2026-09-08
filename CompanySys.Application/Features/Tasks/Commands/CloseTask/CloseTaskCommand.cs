namespace CompanySys.Application.Features.Tasks.Commands.CloseTask;

public record CloseTaskCommand(Guid TaskId) : IRequest<Result>;