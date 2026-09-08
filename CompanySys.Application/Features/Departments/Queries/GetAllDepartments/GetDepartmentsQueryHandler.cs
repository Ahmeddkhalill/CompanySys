using CompanySys.Application.Features.Departments.Queries.GetAllDepartments;


namespace CompanySys.Application.Features.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, Result<List<DepartmentResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<DepartmentResponse>>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
        .AsNoTracking()
        .Select(d => new DepartmentResponse(
            d.Id,
            d.Name,
            d.Description,
            d.Teams.Count))
        .ToListAsync(cancellationToken);

        return Result.Success(departments);
    }
}