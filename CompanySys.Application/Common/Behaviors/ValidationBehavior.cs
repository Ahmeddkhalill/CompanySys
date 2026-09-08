using System.Reflection;

namespace CompanySys.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(
                validator => validator.ValidateAsync(
                    context,
                    cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var errorMessage = string.Join(", ", failures.Select(error => error.ErrorMessage));

        var error = new Error("Validation.Error", errorMessage, 400);

        return CreateFailureResult(error);
    }

    private static TResponse CreateFailureResult(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = typeof(TResponse).GetGenericArguments()[0];

            var failureMethod = typeof(Result)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(method =>
                    method.Name == nameof(Result.Failure) &&
                    method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 1);

            var genericFailureMethod = failureMethod.MakeGenericMethod(valueType);

            return (TResponse)genericFailureMethod.Invoke(null, new object[] { error })!;
        }

        throw new InvalidOperationException($"Unsupported response type: {typeof(TResponse).FullName}");
    }
}