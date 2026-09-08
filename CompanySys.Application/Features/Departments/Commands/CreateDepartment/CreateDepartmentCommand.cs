namespace CompanySys.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(string Name, string? Description) : IRequest<Result<Guid>>;