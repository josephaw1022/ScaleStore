using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{

    options.ForwardDefault = "oidc";

    options.Authority = "http://localhost:8080/auth/realms/mynewrealm";
    options.ClientId = "mynewclient";
    options.ClientSecret = "sOV8KNkaVRSjTKwquXNldQMnIXslppRQ"; // Replace with the actual client secret
    options.ResponseType = "code";
    options.SaveTokens = true;

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization();


builder.AddServiceDefaults();

var app = builder.Build();


app.MapGet("/", () => "Welcome to the application! Access /userinfo for user details or /logout to log out.");


app.MapGet("/userinfo", async Task<string> (HttpContext httpContext) =>
{

    if (!httpContext.User.Identity.IsAuthenticated)
    {
        return "User not authenticated. Please log in.";
    }

    var username = httpContext.User.Identity.Name;
    var userEmail = httpContext.User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

    var claims = httpContext.User.Claims
        .Select(claim => $"{claim.Type}: {claim.Value}")
        .ToList();

    var claimsListString = JsonSerializer.Serialize(claims, new JsonSerializerOptions { WriteIndented = true });

    var accessToken = await httpContext.GetTokenAsync("access_token");

    return $"User Info:\nUsername: {username}\nEmail: {userEmail}\nClaims:\n{claimsListString}\n\nAccess Token:\n\n{accessToken}";

}).RequireAuthorization();


app.MapGet("/logout", async (HttpContext httpContext) =>
{
    // Sign out of the cookie authentication
    await httpContext.SignOutAsync("Cookies");

    // Sign out of the OpenID Connect session
    await httpContext.SignOutAsync("oidc", new AuthenticationProperties
    {
        // Redirect to the home page after logout
        RedirectUri = "/"
    });

    return Results.Redirect("/");
});




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints(); 


app.Run();
