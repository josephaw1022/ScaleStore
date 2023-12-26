using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScaleStoreAuthenticationDb.Auth;





var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();


builder.AddNpgsqlDbContext<AuthDbContext>("authentication");


builder.Services.AddSingleton<AuthDbInitializer>();


builder.Services.AddHostedService(sp => sp.GetRequiredService<AuthDbInitializer>());

builder.Services.AddHealthChecks()
    .AddCheck<AuthDbInitializerHealthCheck>("AuthDbInitializer");


var app = builder.Build();


app.MapDefaultEndpoints();



app.Run();
