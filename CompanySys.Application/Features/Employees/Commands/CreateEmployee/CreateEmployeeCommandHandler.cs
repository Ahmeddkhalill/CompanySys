namespace CompanySys.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<string>>
{
    private readonly IIdentityService _identityService;

    public CreateEmployeeCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        return _identityService.CreateEmployeeAsync(
            request.Email, request.Password, request.FirstName, request.LastName, request.Role, cancellationToken);
    }
}