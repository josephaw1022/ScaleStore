using PreferenceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add MongoDB support
builder.AddMongoDBClient("preference");


builder.Services.AddHttpLogging(o =>
{
    
});

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

app.UseHttpLogging();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
