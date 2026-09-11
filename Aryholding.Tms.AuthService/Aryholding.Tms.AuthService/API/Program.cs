using Aryholding.Tms.AuthService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddPersistence();

builder.Services
    .AddIdentityCoreAuth()
    .AddJwtAuth(builder.Configuration)
    .AddCorsPolicy(builder.Configuration)
    .AddRepositories()
    .AddApplicationServices()
    .AddValidation()
    .AddApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStandardPipeline();

app.Run();
