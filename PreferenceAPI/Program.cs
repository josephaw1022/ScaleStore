using Asp.Versioning;
using PreferenceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add MongoDB support
builder.AddMongoDBClient("preferencedb");


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

// Add Redis support
builder.AddRedisOutputCache("preference-cache");
builder.AddRedisDistributedCache("preference-cache");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpClient ServiceScalingHttpApiService to communicate with the ServiceScalingWebApi
builder.Services.AddHttpClient<ServiceScalingHttpApiService>(client => client.BaseAddress = new("http://scalestore-webapi"));

// Add ProjectPreferenceService to communicate with the MongoDB database
builder.Services.AddScoped<ProjectPreferenceService>();

var app = builder.Build();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
