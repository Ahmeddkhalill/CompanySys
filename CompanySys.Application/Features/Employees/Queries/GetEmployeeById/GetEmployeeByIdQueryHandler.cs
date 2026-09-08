namespace CompanySys.Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    private readonly IIdentityService _identityService;

    public GetEmployeeByIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _identityService.GetEmployeeByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return Result.Failure<EmployeeDto>(EmployeeErrors.NotFound);

        return Result.Success(employee);
    }
}