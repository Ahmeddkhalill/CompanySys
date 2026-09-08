using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class DepartmentErrors
{
    public static readonly Error AlreadyExists = new(
        "Department.AlreadyExists",
        "A department with the same name already exists.",
        409);

    public static readonly Error NotFound = new(
        "Department.NotFound",
        "The requested department was not found.",
        404);

    public static readonly Error HasAssociatedTeams = new(
        "Department.HasAssociatedTeams",
        "Cannot delete department because it contains active teams.",
        400);
}