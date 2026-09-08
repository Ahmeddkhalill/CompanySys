namespace CompanySys.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(100).WithMessage("Team name must not exceed 100 characters.");

        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.TeamLeadId).NotEmpty();
    }
}