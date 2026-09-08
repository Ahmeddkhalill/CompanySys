namespace CompanySys.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RevokeTokenAsync(request.RefreshToken, cancellationToken);
    }
}