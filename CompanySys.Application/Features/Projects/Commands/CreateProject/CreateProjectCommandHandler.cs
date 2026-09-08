using CompanySys.Domain.Entities;
using CompanySys.Domain.Enums;

namespace CompanySys.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Status = ProjectStatus.Pending,
            CustomerId = _currentUserService.UserId!
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(project.Id);
    }
}