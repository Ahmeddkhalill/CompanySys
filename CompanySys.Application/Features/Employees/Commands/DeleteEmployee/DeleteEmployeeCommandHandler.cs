namespace CompanySys.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result>
{
    private readonly IIdentityService _identityService;

    public DeleteEmployeeCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        return _identityService.DeleteEmployeeAsync(request.Id, cancellationToken);
    }
}