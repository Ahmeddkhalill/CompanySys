namespace CompanySys.Application.Features.Projects.Commands.UpdateProjectStatus;

public class UpdateProjectStatusCommandValidator : AbstractValidator<UpdateProjectStatusCommand>
{
    public UpdateProjectStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}