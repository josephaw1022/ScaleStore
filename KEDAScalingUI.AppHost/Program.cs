using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Scaling
var scalingDbApp = builder.AddProject<Projects.ServiceScalingDb>("scalestore-dbapp");
var scaleStoreCache = builder.AddRedisContainer("scalestore-cache", 6379);
var scaleStoreHttpApi = builder.AddProject<Projects.ServiceScalingWebApi>("scalestore-webapi")
    .WithReference(scaleStoreCache);


var secretScaleStoreDbConnectionString = builder.Configuration["scaleStoreDbConnectionString"];

if (builder.Environment.IsDevelopment())
{
    var scalingDb = builder.AddPostgresContainer("scalestore-db", 5433, "JoeMontana3034")
        .AddDatabase("scalestore");

    scaleStoreHttpApi.WithReference(scalingDb);
    scalingDbApp.WithReference(scalingDb);
}
else
{
    if (!string.IsNullOrWhiteSpace(secretScaleStoreDbConnectionString))
    {
        scaleStoreHttpApi.WithEnvironment("ConnectionStrings__scalestore", secretScaleStoreDbConnectionString);
        scalingDbApp.WithEnvironment("ConnectionStrings__scalestore", secretScaleStoreDbConnectionString);
    }

    throw new InvalidOperationException("""
        A password for your external database is not configured.
        Add one to the AppHost project's user secrets with the key 'scaleStoreDbConnectionString', e.g. dotnet user-secrets set scaleStoreDbConnectionString connection string
        """);

}


// Preference Service
var preferenceCache = builder.AddRedisContainer("preference-cache", 6380);

var preferenceDb = builder.AddMongoDBContainer("preference-db", 27017)
    .AddDatabase("preference");

var preferenceHttpApi = builder.AddProject<Projects.PreferenceAPI>("preferenceapi")
                           .WithReference(preferenceDb)
                           .WithReference(scaleStoreHttpApi)
                           .WithReference(preferenceCache);

// web ui
builder.AddProject<Projects.ScaleStoreWebUI>("scalestorewebui")
    .WithReference(scaleStoreHttpApi)
    .WithReference(preferenceHttpApi);

builder.Build().Run();
