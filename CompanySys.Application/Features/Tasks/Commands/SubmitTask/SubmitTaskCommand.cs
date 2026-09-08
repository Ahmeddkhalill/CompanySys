namespace CompanySys.Application.Features.Tasks.Commands.SubmitTask;

public record SubmitTaskCommand(Guid TaskId) : IRequest<Result>;