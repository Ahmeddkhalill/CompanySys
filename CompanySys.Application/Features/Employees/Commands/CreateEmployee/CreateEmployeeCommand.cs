using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(string Email, string Password, string FirstName, string LastName, EmployeeRole Role) : IRequest<Result<string>>;