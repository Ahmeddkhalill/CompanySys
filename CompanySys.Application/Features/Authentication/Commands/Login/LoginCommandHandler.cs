using CompanySys.Application.Contracts.Authentication;

namespace CompanySys.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginRequest = new LoginRequest(request.Email, request.Password);

        return await _authService.LoginAsync(loginRequest, cancellationToken);
    }
}