using Microsoft.Extensions.Configuration;

namespace CompanySys.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            Roles.Owner,
            Roles.TeamLead,
            Roles.Engineer,
            Roles.Customer
        };

        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
            }
        }
    }

    public static async Task SeedRolePermissionsAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var roleData in RolePermissions.Permissions)
        {
            var roleName = roleData.Key;
            var permissions = roleData.Value;

            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
                throw new InvalidOperationException($"Role '{roleName}' was not found.");

            var existingClaims = await roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                var exists = existingClaims.Any(
                    claim => claim.Type == "Permission" && claim.Value == permission);

                if (exists)
                    continue;

                var result = await roleManager.AddClaimAsync(role, new Claim("Permission", permission));

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to add permission '{permission}' to role '{roleName}': {errors}");
                }
            }
        }
    }
    public static async Task SeedOwnerAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        var ownerEmail = configuration["InitialOwner:Email"];
        var ownerPassword = configuration["InitialOwner:Password"];
        var firstName = configuration["InitialOwner:FirstName"] ?? "System";
        var lastName = configuration["InitialOwner:LastName"] ?? "Owner";

        if (string.IsNullOrEmpty(ownerEmail) || string.IsNullOrEmpty(ownerPassword))
        {
            return;
        }

        var existingUser = await userManager.FindByEmailAsync(ownerEmail);

        if (existingUser is null)
        {
            var ownerUser = new ApplicationUser
            {
                UserName = ownerEmail,
                Email = ownerEmail,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(ownerUser, ownerPassword);

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(ownerUser, Roles.Owner);
            }
            else
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to seed initial Owner: {errors}");
            }
        }
    }
    public static async Task SeedTestUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var testUsers = new[]
        {
            new { Email = "teamlead1@companysys.com", Password = "TeamLead123!", FirstName = "Ahmed", LastName = "Hassan", Role = Roles.TeamLead },
            new { Email = "engineer1@companysys.com", Password = "Engineer123!", FirstName = "Mona", LastName = "Ali", Role = Roles.Engineer },
            new { Email = "engineer2@companysys.com", Password = "Engineer123!", FirstName = "Youssef", LastName = "Kamal", Role = Roles.Engineer },
            new { Email = "engineer3@companysys.com", Password = "Engineer123!", FirstName = "Sara", LastName = "Adel", Role = Roles.Engineer }
        };

        foreach (var testUser in testUsers)
        {
            var existingUser = await userManager.FindByEmailAsync(testUser.Email);

            if (existingUser is not null)
                continue;

            var user = new ApplicationUser
            {
                UserName = testUser.Email,
                Email = testUser.Email,
                FirstName = testUser.FirstName,
                LastName = testUser.LastName,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, testUser.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to seed test user '{testUser.Email}': {errors}");
            }

            await userManager.AddToRoleAsync(user, testUser.Role);
        }
    }
}