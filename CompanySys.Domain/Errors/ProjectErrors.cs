using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class ProjectErrors
{
    public static readonly Error NotFound = new("Project.NotFound", "Project was not found.");
    public static readonly Error TeamNotFound = new("Project.TeamNotFound", "The specified team does not exist.");
    public static readonly Error TeamAlreadyAssigned = new("Project.TeamAlreadyAssigned", "This team is already assigned to the project.");
    public static readonly Error TeamNotAssigned = new("Project.TeamNotAssigned", "This team is not assigned to this project.");
    public static readonly Error InvalidStatusTransition = new("Project.InvalidStatusTransition", "This status transition is not allowed.");
    public static readonly Error HasAssociatedTasks = new("Project.HasAssociatedTasks", "Cannot delete a project that has tasks.");
}