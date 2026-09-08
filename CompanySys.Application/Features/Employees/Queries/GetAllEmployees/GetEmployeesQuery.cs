using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Employees.Queries.GetAllEmployees;

public record GetEmployeesQuery(EmployeeRole? Role) : IRequest<Result<List<EmployeeDto>>>;