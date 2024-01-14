var builder = DistributedApplication.CreateBuilder(args);

///Databases

// Postgres Information
var postgresDbPassword = "JoeMontana3034";
var postgresDbPort = 5432;
var postgresDbUser = "postgres";

// Database Containers
var postgres = builder.AddPostgresContainer("postgres", postgresDbPort, postgresDbPassword);
var mongo = builder.AddMongoDBContainer("mongo", 27017);

// Databases
var scalingDb = postgres.AddDatabase("scalestoredb");
var keycloakDb = postgres.AddDatabase("keycloakdb");
var preferenceDb = mongo.AddDatabase("preferencedb");

/// Services

// Authorization Service
var keycloakDbApp = builder.AddProject<Projects.ScaleStoreAuthenticationDb>("authdbinitializer")
    .WithReference(keycloakDb);

var keycloak = builder.AddContainer("keycloak", "jboss/keycloak")
    .WithServiceBinding(containerPort: 8080, hostPort:8080, name: "keycloak-http", scheme: "http")
    .WithReference(keycloakDb)
    .WithReference(keycloakDbApp);

// Define environment variables for the keycloak container
var keyCloakEnvironmentVariables = new Dictionary<string, string>()
{
    ["DB_VENDOR"] = "POSTGRES",
    ["DB_ADDR"] = "host.docker.internal",
    ["DB_DATABASE"] = "keycloakdb",
    ["DB_USER"] = postgresDbUser,
    ["DB_PASSWORD"] = postgresDbPassword,
    ["KEYCLOAK_USER"] = "admin",
    ["KEYCLOAK_PASSWORD"] = "admin",
    ["KC_HOSTNAME_PORT"] = "8080"
};

// Add environment variables to the keycloak container
foreach (KeyValuePair<string, string> kvp in keyCloakEnvironmentVariables)
{
    keycloak.WithEnvironment(kvp.Key, kvp.Value);
}


// Scaling Service
var scalingDbApp = builder.AddProject<Projects.ServiceScalingDb>("scalestore-dbapp")
    .WithReference(scalingDb);

var scaleStoreCache = builder.AddRedisContainer("scalestore-cache", 6379);

var scaleStoreHttpApi = builder.AddProject<Projects.ServiceScalingWebApi>("scalestore-webapi")
    .WithReference(scaleStoreCache)
    .WithReference(scalingDbApp);

// Preference Service
var preferenceCache = builder.AddRedisContainer("preference-cache", 6380);

var preferenceHttpApi = builder.AddProject<Projects.PreferenceAPI>("preferenceapi")
                           .WithReference(preferenceDb)
                           .WithReference(scaleStoreHttpApi)
                           .WithReference(preferenceCache);

// web ui
builder.AddProject<Projects.ScaleStoreWebUI>("scalestorewebui")
    .WithReference(scaleStoreHttpApi)
    .WithReference(preferenceHttpApi);


builder.Build().Run();
