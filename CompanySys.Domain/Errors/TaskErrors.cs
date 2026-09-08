using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class TaskErrors
{
    public static readonly Error NotFound = new("Task.NotFound", "Task was not found.");
    public static readonly Error TeamNotFound = new("Task.TeamNotFound", "The specified team does not exist.");
    public static readonly Error ProjectNotFound = new("Task.ProjectNotFound", "The specified project does not exist.");
    public static readonly Error EngineerNotFound = new("Task.EngineerNotFound", "One or more specified engineers do not exist.");
    public static readonly Error EngineerNotInTeam = new("Task.EngineerNotInTeam", "One or more engineers are not members of this team.");
    public static readonly Error EngineerAlreadyAssigned = new("Task.EngineerAlreadyAssigned", "This engineer is already assigned to the task.");
    public static readonly Error EngineerNotAssigned = new("Task.EngineerNotAssigned", "This engineer is not assigned to this task.");
    public static readonly Error NotAssignedToYou = new("Task.NotAssignedToYou", "You are not assigned to this task.");
    public static readonly Error InvalidProgress = new("Task.InvalidProgress", "Progress must be between 0 and 100.");
    public static readonly Error InvalidStatusForAction = new("Task.InvalidStatusForAction", "The task is not in a valid status for this action.");
    public static readonly Error ProjectNotActive = new("Task.ProjectNotActive", "Cannot create a task for a project that has not been accepted yet.");
    public static readonly Error NoAssignedEngineers = new("Task.NoAssignedEngineers", "Cannot submit a task with no assigned engineers.");
    public static readonly Error DependencyTaskNotFound = new("Task.DependencyTaskNotFound", "The task you're trying to depend on does not exist.");
    public static readonly Error CannotDependOnSelf = new("Task.CannotDependOnSelf", "A task cannot depend on itself.");
    public static readonly Error DependencyAlreadyExists = new("Task.DependencyAlreadyExists", "This dependency already exists.");
    public static readonly Error DependencyNotFound = new("Task.DependencyNotFound", "This dependency does not exist.");
    public static readonly Error CircularDependency = new("Task.CircularDependency", "This would create a circular dependency between tasks.");
    public static readonly Error DependenciesNotCompleted = new("Task.DependenciesNotCompleted", "Cannot submit this task while it has incomplete dependencies.");
}