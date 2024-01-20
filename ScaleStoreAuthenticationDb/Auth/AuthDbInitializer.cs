using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ScaleStoreAuthenticationDb.Auth;

internal sealed class AuthDbInitializer : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<AuthDbInitializer> _logger;
	private readonly ActivitySource _activitySource = new(ActivitySourceName);
	public const string ActivitySourceName = "AuthDbInitialization";

	public AuthDbInitializer(IServiceProvider serviceProvider, ILogger<AuthDbInitializer> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		using var scope = _serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

		await InitializeDatabaseAsync(dbContext, cancellationToken);
	}

	private async Task InitializeDatabaseAsync(AuthDbContext dbContext, CancellationToken cancellationToken)
	{
		var strategy = dbContext.Database.CreateExecutionStrategy();

		using var activity = _activitySource.StartActivity("Initializing authentication database", ActivityKind.Client);

		var sw = Stopwatch.StartNew();

		await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync(cancellationToken));

		// Call seeding methods here
		// Example: await SeedUsersAsync(dbContext, cancellationToken);

		_logger.LogInformation("Authentication database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
	}
}