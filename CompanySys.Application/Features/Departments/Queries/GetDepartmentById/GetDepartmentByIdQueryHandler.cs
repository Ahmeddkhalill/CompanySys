namespace CompanySys.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDetailsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DepartmentDetailsResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .Select(d => new DepartmentDetailsResponse(
                d.Id,
                d.Name,
                d.Description,
                d.Teams.Select(t => new TeamDto(t.Id, t.Name)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
            return Result.Failure<DepartmentDetailsResponse>(DepartmentErrors.NotFound);


        return Result.Success(department);
    }
}