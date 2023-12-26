using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ScaleStoreAuthenticationDb.Auth;

internal sealed class AuthDbInitializerHealthCheck : IHealthCheck
{
    private readonly AuthDbInitializer _dbInitializer;

    public AuthDbInitializerHealthCheck(AuthDbInitializer dbInitializer)
    {
        _dbInitializer = dbInitializer;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var task = _dbInitializer.ExecuteTask;

        return task switch
        {
            { IsCompletedSuccessfully: true } => Task.FromResult(HealthCheckResult.Healthy()),
            { IsFaulted: true } => Task.FromResult(HealthCheckResult.Unhealthy(task.Exception?.InnerException?.Message, task.Exception)),
            { IsCanceled: true } => Task.FromResult(HealthCheckResult.Unhealthy("Authentication database initialization was canceled")),
            _ => Task.FromResult(HealthCheckResult.Degraded("Authentication database initialization is still in progress"))
        };
    }
}
