namespace CompanySys.Application.Features.Projects.Commands.UpdateProjectStatus;

public class UpdateProjectStatusCommandHandler : IRequestHandler<UpdateProjectStatusCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project is null)
            return Result.Failure(ProjectErrors.NotFound);

        project.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}