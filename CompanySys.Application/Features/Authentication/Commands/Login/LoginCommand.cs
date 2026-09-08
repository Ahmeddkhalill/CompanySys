using CompanySys.Application.Contracts.Authentication;

namespace CompanySys.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;