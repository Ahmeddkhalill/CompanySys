using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class TeamErrors
{
    public static readonly Error NotFound = new("Team.NotFound", "Team was not found.");
    public static readonly Error AlreadyExists = new("Team.AlreadyExists", "A team with this name already exists in this department.");
    public static readonly Error DepartmentNotFound = new("Team.DepartmentNotFound", "The specified department does not exist.");
    public static readonly Error TeamLeadNotFound = new("Team.TeamLeadNotFound", "The specified team lead does not exist.");
    public static readonly Error MemberNotFound = new("Team.MemberNotFound", "The specified engineer does not exist.");
    public static readonly Error MemberAlreadyInTeam = new("Team.MemberAlreadyInTeam", "This engineer is already a member of the team.");
    public static readonly Error MemberNotInTeam = new("Team.MemberNotInTeam", "This engineer is not a member of this team.");
    public static readonly Error HasAssociatedWork = new("Team.HasAssociatedWork", "Cannot delete a team that has assigned tasks or projects.");
}