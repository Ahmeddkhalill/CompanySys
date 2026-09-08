namespace CompanySys.Application.Features.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(Guid Id, string Name, string? Description) : IRequest<Result>;