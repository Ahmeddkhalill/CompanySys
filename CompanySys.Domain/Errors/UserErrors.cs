using CompanySys.Domain.Abstractions;

namespace CompanySys.Domain.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email or password.", 401);

    public static readonly Error EmailAlreadyExists =
        new("UserEmailAlreadyExists", "User with this email already exists.", 409);

    public static readonly Error UserNotFound =
        new("User.NotFound", "User was not found.", 404);

    public static readonly Error RoleNotFound =
        new("Role.NotFound", "Default role does not exist.", 500);

    public static readonly Error InvalidToken =
        new("Auth.InvalidToken", "Invalid access token or token claims.", 400);

    public static readonly Error InvalidRefreshToken =
        new("Auth.InvalidRefreshToken", "Refresh token is invalid, expired, or revoked.", 400);

    public static Error RegistrationFailed(string description) =>
        new("User.RegistrationFailed", description, 400);

    public static Error RoleAssignmentFailed(string description) =>
        new("User.RoleAssignmentFailed", description, 500);
}