using Aryholding.Tms.GeneralService.API.Middlewares;

namespace Aryholding.Tms.GeneralService.API.Extensions;

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeneralService API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
