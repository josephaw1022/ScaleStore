using ScaleStoreWebUI.Client.Pages;
using ScaleStoreWebUI.Components;
using ScaleStoreWebUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();




builder.Services.AddScoped<ProjectStateService>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddHttpClient<ScaleStoreApiService>(client => {
    client.BaseAddress = new("http://scalestore-webapi");
    }
);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
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
