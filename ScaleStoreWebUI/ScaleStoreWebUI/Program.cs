using Microsoft.AspNetCore.Components.Server.Circuits;
using ScaleStoreWebUI.Client.Pages;
using ScaleStoreWebUI.Components;
using ScaleStoreWebUI.Services;
using ScaleStoreWebUI.StateManagement;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Redis for state management
builder.AddRedisDistributedCache("scalestore-webui-statestore");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// Add services for state management
builder.Services.AddScoped<CircuitService>();
builder.Services.AddScoped<CircuitHandler, ScaleStoreCircuitHandler>();


builder.Services.AddScoped<StateManagementService>();
builder.Services.AddScoped<ProjectPreferenceStateService>();

// Add http client services
builder.Services.AddHttpClient<ProjectPreferenceApiService>(client => client.BaseAddress = new("http://preferenceapi"));
builder.Services.AddHttpClient<ScaleStoreApiService>(client => client.BaseAddress = new("http://scalestore-webapi"));

// Add Redis for state management
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
