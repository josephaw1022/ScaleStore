var builder = DistributedApplication.CreateBuilder(args);



// Scaling
var scalingDb = builder.AddPostgresContainer("scalestore-db", 5433, "JoeMontana3034")
    .AddDatabase("scalestore");

builder.AddProject<Projects.ServiceScalingDb>("scalestore-dbapp")
                    .WithReference(scalingDb);

var scaleStoreCache = builder.AddRedisContainer("scalestore-cache", 6379);


var scaleStoreHttpApi = builder.AddProject<Projects.ServiceScalingWebApi>("scalestore-webapi")
    .WithReference(scalingDb)
    .WithReference(scaleStoreCache);


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
