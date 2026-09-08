using CompanySys.Domain.Enums;
using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Queries.GetAllTasks;

public record TaskResponse(
    Guid Id, string Title, TaskPriority Priority, DomainTaskStatus Status, int Progress,
    DateTime StartDate, DateTime EndDate, Guid TeamId, Guid ProjectId, List<string> EngineerIds);

public record GetTasksQuery(Guid? TeamId, Guid? ProjectId, DomainTaskStatus? Status) : IRequest<Result<List<TaskResponse>>>;