using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PreferenceDb.Preference;

internal sealed class PreferenceDbInitializerHealthCheck : IHealthCheck
{
	private readonly PreferenceDbInitializer _dbInitializer;

	public PreferenceDbInitializerHealthCheck(PreferenceDbInitializer dbInitializer)
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
			{ IsCanceled: true } => Task.FromResult(HealthCheckResult.Unhealthy("Preference database initialization was canceled")),
			_ => Task.FromResult(HealthCheckResult.Degraded("Preference database initialization is still in progress"))
		};
	}
}