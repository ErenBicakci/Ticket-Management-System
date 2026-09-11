using Aryholding.Tms.GeneralService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddPersistence();
builder.AddRedisCache();
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

app.Run();