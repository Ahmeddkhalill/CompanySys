using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime StartDate,
    DateTime EndDate,
    Guid TeamId,
    Guid ProjectId,
    List<string> EngineerIds) : IRequest<Result<Guid>>;