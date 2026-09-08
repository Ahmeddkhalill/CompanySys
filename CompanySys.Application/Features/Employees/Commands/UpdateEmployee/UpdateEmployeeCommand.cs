using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(string Id, string FirstName, string LastName, EmployeeRole Role) : IRequest<Result>;