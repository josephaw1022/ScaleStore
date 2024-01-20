using PreferenceDb.Preference;

var builder = WebApplication.CreateBuilder(args);

// Add default services
builder.AddServiceDefaults();

// Add Npgsql database context for PreferenceDbContext
builder.AddNpgsqlDbContext<PreferenceDbContext>("preferencedb");

// Register PreferenceDbInitializer as a singleton
builder.Services.AddSingleton<PreferenceDbInitializer>();

// Add PreferenceDbInitializer as a hosted service
builder.Services.AddHostedService(sp => sp.GetRequiredService<PreferenceDbInitializer>());

// Add health checks
builder.Services.AddHealthChecks()
	.AddCheck<PreferenceDbInitializerHealthCheck>("PreferenceDbInitializer");

var app = builder.Build();

// Map default endpoints (e.g., for health checks)
app.MapDefaultEndpoints();

app.Run();