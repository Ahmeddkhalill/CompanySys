using CompanySys.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CompanySys.Application.Common.Behaviors;

public class AuditLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly string[] KnownActions =
    [
        "Create", "Update", "Delete", "Assign", "Remove", "Add",
        "Submit", "Review", "Close"
    ];

    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<AuditLoggingBehavior<TRequest, TResponse>> _logger;

    public AuditLoggingBehavior(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        ILogger<AuditLoggingBehavior<TRequest, TResponse>> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = typeof(TRequest).Name;

        if (!typeName.EndsWith("Command"))
        {
            return await next();
        }

        var (action, entityName) = ParseCommandName(typeName);
        var requestJson = SerializeRequest(request);

        var isSuccess = true;
        string? errorMessage = null;
        string? entityId = TryGetRequestId(request);
        TResponse response;

        try
        {
            response = await next();

            isSuccess = IsResultSuccess(response, out errorMessage, out var responseId);

            entityId ??= responseId;

            return response;
        }
        catch (Exception ex)
        {
            isSuccess = false;
            errorMessage = ex.Message;
            throw;
        }
        finally
        {
            _logger.LogInformation(
                "Audit: {Action} {EntityName} by {UserId} - Success: {IsSuccess}",
                action, entityName, _currentUserService.UserId ?? "Anonymous", isSuccess);

            var details = JsonSerializer.Serialize(new
            {
                Request = requestJson,
                IsSuccess = isSuccess,
                Error = errorMessage
            });

            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = _currentUserService.UserId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static (string Action, string EntityName) ParseCommandName(string typeName)
    {
        var nameWithoutSuffix = typeName.EndsWith("Command")
            ? typeName[..^"Command".Length]
            : typeName;

        foreach (var known in KnownActions)
        {
            if (nameWithoutSuffix.StartsWith(known, StringComparison.Ordinal))
            {
                var rest = nameWithoutSuffix[known.Length..];
                return (known, string.IsNullOrEmpty(rest) ? "Unknown" : rest);
            }
        }

        return (nameWithoutSuffix, "Unknown");
    }

    private static string? SerializeRequest(TRequest request)
    {
        try
        {
            return JsonSerializer.Serialize(request);
        }
        catch
        {
            return null;
        }
    }

    private static string? TryGetRequestId(TRequest request)
    {
        var idProperty = typeof(TRequest).GetProperty("Id");
        var value = idProperty?.GetValue(request);
        return value?.ToString();
    }

    private static bool IsResultSuccess(TResponse response, out string? errorMessage, out string? responseId)
    {
        errorMessage = null;
        responseId = null;

        var isSuccessProperty = typeof(TResponse).GetProperty("IsSuccess");

        if (isSuccessProperty is null)
            return true;

        var isSuccess = (bool)(isSuccessProperty.GetValue(response) ?? true);

        if (!isSuccess)
        {
            var errorProperty = typeof(TResponse).GetProperty("Error");
            errorMessage = errorProperty?.GetValue(response)?.ToString();
        }
        else
        {
            var valueProperty = typeof(TResponse).GetProperty("Value");
            var value = valueProperty?.GetValue(response);

            if (value is Guid guidValue)
                responseId = guidValue.ToString();
            else if (value is string stringValue)
                responseId = stringValue;
        }

        return isSuccess;
    }
}