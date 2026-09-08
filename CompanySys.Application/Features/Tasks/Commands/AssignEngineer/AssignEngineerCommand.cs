namespace CompanySys.Application.Features.Tasks.Commands.AssignEngineer;

public record AssignEngineerCommand(Guid TaskId, string EngineerId) : IRequest<Result>;