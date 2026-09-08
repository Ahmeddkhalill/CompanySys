namespace CompanySys.Application.Features.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(Guid Id) : IRequest<Result>;