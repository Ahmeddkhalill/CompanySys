using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class EmployeeErrors
{
    public static readonly Error NotFound = new("Employee.NotFound", "Employee was not found.");
    public static readonly Error EmailAlreadyExists = new("Employee.EmailAlreadyExists", "An employee with this email already exists.");
    public static Error CreationFailed(string details) => new("Employee.CreationFailed", $"Failed to create employee: {details}");
    public static Error UpdateFailed(string details) => new("Employee.UpdateFailed", $"Failed to update employee: {details}");
    public static Error DeleteFailed(string details) => new("Employee.DeleteFailed", $"Failed to delete employee: {details}");
}