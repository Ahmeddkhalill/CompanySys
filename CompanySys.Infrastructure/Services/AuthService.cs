using CompanySys.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace CompanySys.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ApplicationDbContext _context;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ApplicationDbContext context,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return Result.Failure<AuthResponse>(
                UserErrors.EmailAlreadyExists);
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
        };

        var createResult =
            await _userManager.CreateAsync(
                user,
                request.Password);

        if (!createResult.Succeeded)
        {
            var error = createResult.Errors.First();

            return Result.Failure<AuthResponse>(
                UserErrors.RegistrationFailed(
                    error.Description));
        }

        var defaultRole = Roles.Customer;

        if (!await _roleManager.RoleExistsAsync(defaultRole))
        {
            return Result.Failure<AuthResponse>(
                UserErrors.RoleNotFound);
        }

        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                defaultRole);

        if (!roleResult.Succeeded)
        {
            var error = roleResult.Errors.First();

            return Result.Failure<AuthResponse>(
                UserErrors.RoleAssignmentFailed(
                    error.Description));
        }

        return await GenerateAuthResponseAsync(
            user,
            cancellationToken);
    }

    public async Task<Result<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure<AuthResponse>(
                UserErrors.InvalidCredentials);
        }

        var isPasswordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password);

        if (!isPasswordValid)
        {
            return Result.Failure<AuthResponse>(
                UserErrors.InvalidCredentials);
        }

        return await GenerateAuthResponseAsync(
            user,
            cancellationToken);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.Token);

        if (principal is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var refreshToken = await _context.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == request.RefreshToken && r.UserId == userId, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAt <= DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        return await GenerateAuthResponseAsync(user, cancellationToken);
    }

    public async Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var token = await _context.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == refreshToken, cancellationToken);

        if (token is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        if (token.IsRevoked)
            return Result.Success();


        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result<AuthResponse>> GenerateAuthResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var permissions = await GetPermissionsAsync(roles);

        var (token, expiresIn) =
            await _jwtTokenGenerator.GenerateTokenAsync(user.Id, user.Email!, user.FirstName, user.LastName, roles, permissions);

        var refreshTokenString = _jwtTokenGenerator.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = user.Id,
            ExpiresAt =
            DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays),
            IsRevoked = false
        };

        _context.Set<RefreshToken>().Add(refreshToken);

        await _context.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            token,
            expiresIn,
            refreshTokenString,
            refreshToken.ExpiresAt);

        return Result.Success(response);
    }

    private async Task<List<Claim>> GetPermissionsAsync(IList<string> roles)
    {
        var permissions = new List<Claim>();

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
                continue;

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            permissions.AddRange(roleClaims.Where(claim => claim.Type == "Permission"));
        }

        return permissions
            .GroupBy(
                claim => new
                {
                    claim.Type,
                    claim.Value
                })
            .Select(
                group => group.First())
            .ToList();
    }
}