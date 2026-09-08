namespace CompanySys.Application.Features.Tasks.Commands.UpdateTaskProgress;

public class UpdateTaskProgressCommandValidator : AbstractValidator<UpdateTaskProgressCommand>
{
    public UpdateTaskProgressCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
        RuleFor(x => x.Progress).InclusiveBetween(0, 100);
    }
}