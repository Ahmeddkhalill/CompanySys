using CompanySys.Application.Features.Employees.Queries;
using CompanySys.Domain.Enums;

namespace CompanySys.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> CreateEmployeeAsync(
        string email, string password, string firstName, string lastName, EmployeeRole role,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
            return Result.Failure<string>(EmployeeErrors.EmailAlreadyExists);

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<string>(EmployeeErrors.CreationFailed(errors));
        }

        var roleName = MapRole(role);
        var roleResult = await _userManager.AddToRoleAsync(user, roleName);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<string>(EmployeeErrors.CreationFailed(errors));
        }

        return Result.Success(user.Id);
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user is not null;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync(EmployeeRole? role, CancellationToken cancellationToken = default)
    {
        List<ApplicationUser> users;

        if (role.HasValue)
        {
            var roleName = MapRole(role.Value);
            users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
        }
        else
        {
            var teamLeads = await _userManager.GetUsersInRoleAsync(Roles.TeamLead);
            var engineers = await _userManager.GetUsersInRoleAsync(Roles.Engineer);
            users = teamLeads.Concat(engineers).ToList();
        }

        var result = new List<EmployeeDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new EmployeeDto(user.Id, user.Email!, user.FirstName, user.LastName, roles.FirstOrDefault() ?? string.Empty));
        }

        return result;
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new EmployeeDto(user.Id, user.Email!, user.FirstName, user.LastName, roles.FirstOrDefault() ?? string.Empty);
    }

    public async Task<Result> UpdateEmployeeAsync(string userId, string firstName, string lastName, EmployeeRole role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(EmployeeErrors.NotFound);

        user.FirstName = firstName;
        user.LastName = lastName;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            return Result.Failure(EmployeeErrors.UpdateFailed(errors));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var newRoleName = MapRole(role);

        if (!currentRoles.Contains(newRoleName))
        {
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, newRoleName);
        }

        return Result.Success();
    }

    public async Task<Result> DeleteEmployeeAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(EmployeeErrors.NotFound);

        var deleteResult = await _userManager.DeleteAsync(user);

        if (!deleteResult.Succeeded)
        {
            var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
            return Result.Failure(EmployeeErrors.DeleteFailed(errors));
        }

        return Result.Success();
    }

    private static string MapRole(EmployeeRole role) => role switch
    {
        EmployeeRole.TeamLead => Roles.TeamLead,
        EmployeeRole.Engineer => Roles.Engineer,
        _ => throw new ArgumentOutOfRangeException(nameof(role))
    };
}