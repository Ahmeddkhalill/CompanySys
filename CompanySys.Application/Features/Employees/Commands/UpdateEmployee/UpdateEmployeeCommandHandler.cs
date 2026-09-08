namespace CompanySys.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result>
{
    private readonly IIdentityService _identityService;

    public UpdateEmployeeCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        return _identityService.UpdateEmployeeAsync(request.Id, request.FirstName, request.LastName, request.Role, cancellationToken);
    }
}