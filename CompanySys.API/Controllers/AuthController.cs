using CompanySys.Application.Contracts.Authentication;
using CompanySys.Application.Features.Authentication.Commands.Login;
using CompanySys.Application.Features.Authentication.Commands.Logout;
using CompanySys.Application.Features.Authentication.Commands.RefreshToken;
using CompanySys.Application.Features.Authentication.Commands.Register;

namespace CompanySys.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(FirstName: request.FirstName, LastName: request.LastName, Email: request.Email, Password: request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.Token, request.RefreshToken);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}