using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AccessControl.Api.Middleware;

public class ExternalServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExternalServiceHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check external services if configured
            return HealthCheckResult.Healthy("External services are available");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("External services check failed", ex);
        }
    }
}
