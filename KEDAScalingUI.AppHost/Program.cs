using KEDAScalingUI.AppHost;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// Load configuration from a file such as appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Create and load Keycloak configuration
var keyCloakConfig = new KeycloakConfig();
configuration.GetSection("Keycloak").Bind(keyCloakConfig);


///Databases

// Postgres Information
var postgresDbPassword = "JoeMontana3034";
var postgresDbPort = 5432;


// Databases
var postgres = builder.AddPostgresContainer("postgres", postgresDbPort, postgresDbPassword);

var scalingDb = postgres.AddDatabase("scalestoredb");
var keycloakDb = postgres.AddDatabase("keycloakdb");
var preferenceDb = postgres.AddDatabase("preferencedb");

/// Services

// Authorization Service
var keycloakDbApp = builder.AddProject<Projects.ScaleStoreAuthenticationDb>("authdbinitializer")
    .WithReference(keycloakDb);

var keycloak = builder.AddContainer("keycloak", "jboss/keycloak")
    .WithServiceBinding(containerPort: 8080, hostPort:8080, name: "keycloak-http", scheme: "http")
    .WithReference(keycloakDb)
    .WithReference(keycloakDbApp);

// Directly use keyCloakConfig to set environment variables for the keycloak container
keycloak.WithEnvironment("DB_VENDOR", keyCloakConfig.DB_VENDOR)
        .WithEnvironment("DB_ADDR", keyCloakConfig.DB_HOST)
        .WithEnvironment("DB_DATABASE", keyCloakConfig.DB_DATABASE)
        .WithEnvironment("DB_USER", keyCloakConfig.DB_USER)
        .WithEnvironment("DB_PASSWORD", keyCloakConfig.DB_PASSWORD)
        .WithEnvironment("KEYCLOAK_USER", keyCloakConfig.KEYCLOAK_USER)
        .WithEnvironment("KEYCLOAK_PASSWORD", keyCloakConfig.KEYCLOAK_PASSWORD)
        .WithEnvironment("KC_HOSTNAME_PORT", keyCloakConfig.KC_HOSTNAME_PORT);

// Scaling Service
var scalingDbApp = builder.AddProject<Projects.ServiceScalingDb>("scalestore-dbapp")
    .WithReference(scalingDb);

var scaleStoreCache = builder.AddRedisContainer("scalestore-cache", 6379);

var scaleStoreHttpApi = builder.AddProject<Projects.ServiceScalingWebApi>("scalestore-webapi")
    .WithReference(scaleStoreCache)
    .WithReference(scalingDbApp)
    .WithReference(scalingDb);

// Preference Service
var preferenceCache = builder.AddRedisContainer("preference-cache", 6380);

var preferenceDbApp = builder.AddProject<Projects.PreferenceDb>("preferencedbapp")
    .WithReference(preferenceDb);

var preferenceHttpApi = builder.AddProject<Projects.PreferenceAPI>("preferenceapi")
                           .WithReference(preferenceDb)
                           .WithReference(scaleStoreHttpApi)
                           .WithReference(preferenceDbApp)
                           .WithReference(preferenceCache);

// web ui
builder.AddProject<Projects.ScaleStoreWebUI>("scalestorewebui")
    .WithReference(scaleStoreHttpApi)
    .WithReference(preferenceHttpApi);


builder.Build().Run();
