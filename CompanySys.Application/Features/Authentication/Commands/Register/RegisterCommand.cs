using CompanySys.Application.Contracts.Authentication;

namespace CompanySys.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<Result<AuthResponse>>;