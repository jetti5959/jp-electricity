// Azure Functions Isolated Worker namespace
using Microsoft.Azure.Functions.Worker.Builder;

// Entity Framework Core namespace (used for PostgreSQL database connection)
using Microsoft.EntityFrameworkCore;

// Dependency Injection namespace
using Microsoft.Extensions.DependencyInjection;

// Used to build and run the Azure Functions application
using Microsoft.Extensions.Hosting;

// Used to read configuration values from local.settings.json or Azure App Settings
using Microsoft.Extensions.Configuration;

// Project namespaces
using JPAzureFunctions.Data;
using JPAzureFunctions.Services;
using JPAzureFunctions.Services.Interfaces;

// -------------------------
// ENTRY POINT OF THE PROJECT
// -------------------------
// Creates the Azure Functions application builder.
// This is the first line executed when the Function App starts.
var builder = FunctionsApplication.CreateBuilder(args);

// -------------------------
// CONFIGURE AZURE FUNCTIONS
// -------------------------
// Enables HTTP Trigger support and configures the Azure Functions web application pipeline.
builder.ConfigureFunctionsWebApplication();

// -------------------------
// DATABASE CONFIGURATION
// -------------------------
// Registers AppDbContext in Dependency Injection.
// Reads the PostgreSQL connection string from:
// Local  -> local.settings.json
// Azure  -> ConnectionStrings__DefaultConnection (App Settings)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:DefaultConnection"]));

// -------------------------
// DEPENDENCY INJECTION
// -------------------------
// Registers AuthService with its interface.
// Whenever IAuthService is required, Azure automatically creates an AuthService object.
builder.Services.AddScoped<IAuthService, AuthService>();

// -------------------------
// BUILD THE APPLICATION
// -------------------------
// Creates the dependency injection container and prepares the Function App to run.
var app = builder.Build();

// -------------------------
// START THE FUNCTION APP
// -------------------------
// Starts the Azure Functions runtime and begins listening for HTTP requests.
app.Run();