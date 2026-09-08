using CompanySys.Application.Contracts.Authentication;

namespace CompanySys.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterRequest(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            Password: request.Password
        );

        return await _authService.RegisterAsync(registerRequest, cancellationToken);
    }
}