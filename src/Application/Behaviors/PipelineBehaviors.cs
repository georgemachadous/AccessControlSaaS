using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AccessControl.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation failed for {RequestType}: {Errors}", typeof(TRequest).Name, string.Join(", ", failures.Select(f => f.ErrorMessage)));
            throw new ValidationException(failures);
        }

        return await next();
    }
}

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var correlationId = Guid.NewGuid().ToString()[..8];
        _logger.LogInformation("[{CorrelationId}] Handling {RequestName}", correlationId, requestName);
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();
        _logger.LogInformation("[{CorrelationId}] Completed {RequestName} in {ElapsedMs}ms", correlationId, requestName, stopwatch.ElapsedMilliseconds);
        return response;
    }
}

public class TenantBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Domain.Interfaces.ITenantContext _tenantContext;
    private readonly ILogger<TenantBehavior<TRequest, TResponse>> _logger;

    public TenantBehavior(Domain.Interfaces.ITenantContext tenantContext, ILogger<TenantBehavior<TRequest, TResponse>> logger)
    {
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_tenantContext.IsAuthenticated && _tenantContext.CurrentTenantId == Guid.Empty)
        {
            _logger.LogWarning("Request {RequestType} rejected: no tenant context", typeof(TRequest).Name);
            throw new UnauthorizedAccessException("Tenant context required");
        }
        return await next();
    }
}
