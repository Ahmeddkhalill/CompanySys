namespace CompanySys.Application.Features.Tasks.Commands.ReviewTask;

public record ReviewTaskCommand(Guid TaskId, bool Approved) : IRequest<Result>;