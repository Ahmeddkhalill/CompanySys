namespace CompanySys.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
        {
            return Result.Failure(DepartmentErrors.NotFound);
        }

        var nameExists = await _context.Departments
            .AnyAsync(d => d.Name.ToLower() == request.Name.ToLower() && d.Id != request.Id, cancellationToken);

        if (nameExists)
        {
            return Result.Failure(DepartmentErrors.AlreadyExists);
        }

        department.Name = request.Name;
        department.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}