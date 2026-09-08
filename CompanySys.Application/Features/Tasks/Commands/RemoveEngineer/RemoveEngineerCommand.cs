namespace CompanySys.Application.Features.Tasks.Commands.RemoveEngineer;

public record RemoveEngineerCommand(Guid TaskId, string EngineerId) : IRequest<Result>;