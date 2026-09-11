using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace Aryholding.Tms.GeneralService.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ClearCacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _cacheKey;

        public ClearCacheAttribute(string cacheKey)
        {
            _cacheKey = cacheKey;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Exception == null || executedContext.ExceptionHandled)
            {
                var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
                await cache.RemoveAsync(_cacheKey);
            }
        }
    }
}
