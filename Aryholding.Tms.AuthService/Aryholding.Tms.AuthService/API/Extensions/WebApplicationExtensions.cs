using Aryholding.Tms.AuthService.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Aryholding.Tms.AuthService.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseStandardPipeline(this WebApplication app)
        {
            app.UseGlobalExceptionHandling();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
