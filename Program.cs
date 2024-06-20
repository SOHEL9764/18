using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApp.Model; // Make sure to use your correct namespace

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
}

// Register DAL as a service
builder.Services.AddSingleton<DAL>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Example endpoint to demonstrate usage of DAL
app.MapGet("/users", (DAL dal, IConfiguration config) =>
{
    return dal.GetUsers(config);
});

app.Run();
