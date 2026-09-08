namespace CompanySys.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(150).WithMessage("Project name must not exceed 150 characters.");

        RuleFor(x => x.Description).MaximumLength(1000);
    }
}