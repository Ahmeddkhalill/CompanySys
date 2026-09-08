namespace CompanySys.Infrastructure.Identity;

public static class PermissionsList
{
    public static class Departments
    {
        public const string Create = "Departments.Create";
        public const string Read = "Departments.Read";
        public const string Update = "Departments.Update";
        public const string Delete = "Departments.Delete";
    }

    public static class Teams
    {
        public const string Create = "Teams.Create";
        public const string Read = "Teams.Read";
        public const string Update = "Teams.Update";
        public const string Delete = "Teams.Delete";
        public const string ManageMembers = "Teams.ManageMembers";
    }

    public static class Projects
    {
        public const string Create = "Projects.Create";
        public const string Read = "Projects.Read";
        public const string Update = "Projects.Update";
        public const string Delete = "Projects.Delete";
        public const string TrackProgress = "Projects.TrackProgress";
    }

    public static class Tasks
    {
        public const string Create = "Tasks.Create";
        public const string ReadAssigned = "Tasks.ReadAssigned";
        public const string Assign = "Tasks.Assign";
        public const string UpdateProgress = "Tasks.UpdateProgress";
        public const string SubmitResult = "Tasks.SubmitResult";
        public const string ReviewSubmission = "Tasks.ReviewSubmission";
        public const string Complete = "Tasks.Complete";
    }

    public static class Employees
    {
        public const string Create = "Employees.Create";
        public const string Read = "Employees.Read";
        public const string Update = "Employees.Update";
        public const string Delete = "Employees.Delete";
    }
    public static class AuditLogs
    {
        public const string Read = "AuditLogs.Read";
    }
}