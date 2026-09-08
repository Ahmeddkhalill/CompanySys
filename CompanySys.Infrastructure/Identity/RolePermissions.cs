namespace CompanySys.Infrastructure.Identity;

public static class RolePermissions
{
    public static readonly Dictionary<string, string[]> Permissions =
        new()
        {
            [Roles.Owner] =
            [
                PermissionsList.Departments.Create,
                PermissionsList.Departments.Read,
                PermissionsList.Departments.Update,
                PermissionsList.Departments.Delete,

                PermissionsList.Teams.Create,
                PermissionsList.Teams.Read,
                PermissionsList.Teams.Update,
                PermissionsList.Teams.Delete,
                PermissionsList.Teams.ManageMembers,

                PermissionsList.Projects.Create,
                PermissionsList.Projects.Read,
                PermissionsList.Projects.Update,
                PermissionsList.Projects.Delete,
                PermissionsList.Projects.TrackProgress,

                PermissionsList.Tasks.Create,
                PermissionsList.Tasks.ReadAssigned,
                PermissionsList.Tasks.Assign,
                PermissionsList.Tasks.UpdateProgress,
                PermissionsList.Tasks.SubmitResult,
                PermissionsList.Tasks.ReviewSubmission,
                PermissionsList.Tasks.Complete,

                Identity.PermissionsList.AuditLogs.Read,

                PermissionsList.Employees.Create,
                PermissionsList.Employees.Read,
                PermissionsList.Employees.Update,
                PermissionsList.Employees.Delete
            ],

            [Roles.TeamLead] =
            [
                PermissionsList.Teams.Read,
                PermissionsList.Teams.ManageMembers,

                PermissionsList.Projects.Read,
                PermissionsList.Projects.Update,

                PermissionsList.Tasks.Create,
                PermissionsList.Tasks.ReadAssigned,
                PermissionsList.Tasks.Assign,
                PermissionsList.Tasks.UpdateProgress,
                PermissionsList.Tasks.ReviewSubmission,
                PermissionsList.Tasks.Complete,

                PermissionsList.Employees.Read
            ],

            [Roles.Engineer] =
            [
                PermissionsList.Teams.Read,

                PermissionsList.Tasks.ReadAssigned,
                PermissionsList.Tasks.UpdateProgress,
                PermissionsList.Tasks.SubmitResult
            ],

            [Roles.Customer] =
            [
                PermissionsList.Projects.Create,
                PermissionsList.Projects.Read,
                PermissionsList.Projects.TrackProgress
            ]
        };
}