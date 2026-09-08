using CompanySys.API.Authorization;
using CompanySys.Application.Features.AuditLogs.Queries.GetAuditLogs;

namespace CompanySys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditLogsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpGet]
    [HasPermission(PermissionsList.AuditLogs.Read)]
    public async Task<IActionResult> GetAll([FromQuery] string? userId, [FromQuery] string? action, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAuditLogsQuery(userId, action, page, pageSize), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}