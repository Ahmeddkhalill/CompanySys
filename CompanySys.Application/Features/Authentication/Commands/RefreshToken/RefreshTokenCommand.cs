using CompanySys.Application.Contracts.Authentication;

namespace CompanySys.Application.Features.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<Result<AuthResponse>>;