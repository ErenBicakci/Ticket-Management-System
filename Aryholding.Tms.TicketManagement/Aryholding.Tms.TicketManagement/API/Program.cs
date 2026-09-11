using Aryholding.Tms.TicketManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddPersistence();
builder.AddMessaging();

builder.Services
    .AddIdentityCoreAuth()
    .AddJwtAuth(builder.Configuration)
    .AddRepositories()
    .AddApplicationServices()
    .AddCorsPolicy(builder.Configuration)
    .AddValidation()
    .AddApiAndSwagger();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStandardPipeline();

await app.MigrateAndSeedAsync();

app.Run();
