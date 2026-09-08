using CompanySys.Domain.Enums;
using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Queries.GetTaskById;

public record TaskDependencyDto(Guid Id, string Title, DomainTaskStatus Status);

public record TaskDetailsResponse(
    Guid Id, string Title, string? Description, TaskPriority Priority, DomainTaskStatus Status, int Progress,
    DateTime StartDate, DateTime EndDate, Guid TeamId, Guid ProjectId, List<string> EngineerIds,
    List<TaskDependencyDto> Dependencies);

public record GetTaskByIdQuery(Guid Id) : IRequest<Result<TaskDetailsResponse>>;