using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateTeamCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            return Result.Failure<Guid>(TeamErrors.DepartmentNotFound);

        var teamLeadExists = await _identityService.UserExistsAsync(request.TeamLeadId, cancellationToken);

        if (!teamLeadExists)
            return Result.Failure<Guid>(TeamErrors.TeamLeadNotFound);

        var nameExists = await _context.Teams
            .AnyAsync(t => t.DepartmentId == request.DepartmentId && t.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (nameExists)
            return Result.Failure<Guid>(TeamErrors.AlreadyExists);

        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DepartmentId = request.DepartmentId,
            TeamLeadId = request.TeamLeadId
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(team.Id);
    }
}