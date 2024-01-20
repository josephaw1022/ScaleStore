

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ServiceScalingDb.ScalingDb;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ScalingDbContext>("scalestoredb");

builder.Services.AddSingleton<ScalingDbInitializer>();


builder.Services.AddHostedService(sp => sp.GetRequiredService<ScalingDbInitializer>());

builder.Services.AddHealthChecks()
	.AddCheck<ScalingDbInitializerHealthCheck>("ScalingDbInitializer");


var app = builder.Build();


app.MapDefaultEndpoints();


app.Run();