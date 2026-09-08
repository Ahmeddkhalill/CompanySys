namespace CompanySys.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(string Id) : IRequest<Result>;