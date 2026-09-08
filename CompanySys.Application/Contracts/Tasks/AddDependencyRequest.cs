namespace CompanySys.Application.Contracts.Tasks;

public record AddDependencyRequest(Guid DependsOnTaskId);