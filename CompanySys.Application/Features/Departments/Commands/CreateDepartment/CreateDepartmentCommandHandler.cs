using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Departments.AnyAsync(d => d.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return Result.Failure<Guid>(DepartmentErrors.AlreadyExists);

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CompanyId = SeedIds.DefaultCompanyId
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(department.Id);
    }
}