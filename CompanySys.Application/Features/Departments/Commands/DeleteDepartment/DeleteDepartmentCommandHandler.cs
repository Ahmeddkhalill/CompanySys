namespace CompanySys.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
        {
            return Result.Failure(DepartmentErrors.NotFound);
        }

        if (department.Teams.Any())
        {
            return Result.Failure(DepartmentErrors.HasAssociatedTeams);
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}