using Asp.Versioning;
using Microsoft.Extensions.Hosting;
using ServiceScalingDb.ScalingDb;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisOutputCache("scalestore-cache");
builder.AddRedisDistributedCache("scalestore-cache");
builder.AddRedis("scalestore-cache");


builder.Services.AddApiVersioning(
                    options =>
                    {
                        // Specify the default API Version as 1.0
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        // Reporting API versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                        options.ReportApiVersions = true;
                        // Assume that the client is requesting the default version if they don't specify a version
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.ReportApiVersions = true;
                    })
    .AddMvc();


builder.AddNpgsqlDbContext<ScalingDbContext>("scalestoredb");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.UseOutputCache();

app.MapControllers();

app.Run();
