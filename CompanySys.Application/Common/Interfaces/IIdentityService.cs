using CompanySys.Application.Features.Employees.Queries;
using CompanySys.Domain.Enums;

namespace CompanySys.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<string>> CreateEmployeeAsync(
        string email, string password, string firstName, string lastName, EmployeeRole role,
        CancellationToken cancellationToken = default);

    Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);

    Task<List<EmployeeDto>> GetEmployeesAsync(EmployeeRole? role, CancellationToken cancellationToken = default);

    Task<EmployeeDto?> GetEmployeeByIdAsync(string userId, CancellationToken cancellationToken = default);

    Task<Result> UpdateEmployeeAsync(
        string userId, string firstName, string lastName, EmployeeRole role,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteEmployeeAsync(string userId, CancellationToken cancellationToken = default);
}