namespace CompanySys.Application.Features.Employees.Queries.GetAllEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<List<EmployeeDto>>>
{
    private readonly IIdentityService _identityService;

    public GetEmployeesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<List<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _identityService.GetEmployeesAsync(request.Role, cancellationToken);
        return Result.Success(employees);
    }
}