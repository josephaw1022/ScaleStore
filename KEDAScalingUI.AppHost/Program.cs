var builder = DistributedApplication.CreateBuilder(args);


// Authentication/Authorization

var authenticationDb = builder.AddPostgresContainer("authentication-db", 5432)
                              .AddDatabase("authentication");

var authenticationdbApp = builder.AddProject<Projects.ScaleStoreAuthenticationDb>("authentication-dbapp")
    .WithReference(authenticationDb);

var authenticationHttpApi = builder.AddProject<Projects.ScaleStoreAuthenticationWebApi>("authentication-webapi")
    .WithReference(authenticationDb);

// Scaling
var scalingDb = builder.AddPostgresContainer("scalestore-db", 5433)
    .AddDatabase("scalestore");

var scalingdbApp = builder.AddProject<Projects.ServiceScalingDb>("scalestore-dbapp")
                    .WithReference(scalingDb);

var scaleStoreHttpApi = builder.AddProject<Projects.ServiceScalingWebApi>("scalestore-webapi")
    .WithReference(scalingDb)
    .WithReference(authenticationHttpApi);



// Preference Service
var preferenceDb = builder.AddMongoDBContainer("preference-db", 27017)
    .AddDatabase("preference");

var preferenceHttpApi = builder.AddProject<Projects.PreferenceAPI>("preferenceapi")
                           .WithReference(preferenceDb)
                           .WithReference(scaleStoreHttpApi);

// web ui
var webUi = builder.AddProject<Projects.ScaleStoreWebUI>("scalestorewebui")
    .WithReference(scaleStoreHttpApi)
    .WithReference(preferenceHttpApi);

builder.Build().Run();
