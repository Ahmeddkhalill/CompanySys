namespace CompanySys.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(string Id) : IRequest<Result<EmployeeDto>>;