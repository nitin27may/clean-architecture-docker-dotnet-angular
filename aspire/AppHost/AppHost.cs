var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL with pgAdmin
var postgresPassword = builder.AddParameter("postgres-password", secret: true);

// Get the path to scripts folder relative to the solution
var scriptsPath = Path.Combine(builder.AppHostDirectory, "..", "..", "scripts");

var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume("postgres-data")
    .WithPgAdmin()
    .WithBindMount(scriptsPath, "/docker-entrypoint-initdb.d");

var contactsDb = postgres.AddDatabase("contactsdb", "contacts");

// Backend API - reference the PostgreSQL database
var api = builder.AddProject<Projects.Contact_Api>("contact-api")
    .WithReference(contactsDb)
    .WaitFor(contactsDb)
    .WithEnvironment(context =>
    {
        context.EnvironmentVariables["AppSettings__ConnectionStrings__DefaultConnection"] = contactsDb.Resource.ConnectionStringExpression;
    });

// Angular Frontend
var frontend = builder.AddNpmApp("frontend", "../../frontend", "serve")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(targetPort: 4200, env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
