namespace CompanySys.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(100).WithMessage("Team name must not exceed 100 characters.");
        RuleFor(x => x.TeamLeadId).NotEmpty();
    }
}