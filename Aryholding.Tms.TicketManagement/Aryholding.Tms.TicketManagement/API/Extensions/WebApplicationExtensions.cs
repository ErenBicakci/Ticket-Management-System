using Aryholding.Tms.TicketManagement.API.Middlewares;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aryholding.Tms.TicketManagement.API.Extensions;

public static class WebApplicationExtensions
{


    public static WebApplication UseStandardPipeline(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrudService API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseCors("AllowAll");
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static async Task<WebApplication> MigrateAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TicketDbContext>();

        try
        {
            await db.Database.MigrateAsync();
            await DataSeeder.SeedAsync(db, app.Logger);
            app.Logger.LogInformation("DB migrated & seeded.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "DB migration/seed error");
        }

        return app;
    }
}
