using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Aryholding.Tms.GeneralService.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RedisCacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expirationMinutes;
        private readonly string _cacheKey;
        private readonly bool _usernameRequired;

        public RedisCacheAttribute(int expirationMinutes = 5, string? cacheKey = null, bool usernameRequired = false)
        {
            _expirationMinutes = expirationMinutes;
            _cacheKey = cacheKey ?? string.Empty;
            _usernameRequired = usernameRequired;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var key = GenerateCacheKey(context);
            var cachedResponse = await cache.GetStringAsync(key);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var result = JsonSerializer.Deserialize<object>(cachedResponse, jsonOptions);
                context.Result = new OkObjectResult(result);
                return;
            }

            var executedResult = await next();

            if (executedResult.Result is OkObjectResult okResult)
            {
                var responseJson = JsonSerializer.Serialize(okResult.Value, jsonOptions);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_expirationMinutes)
                };
                await cache.SetStringAsync(key, responseJson, options);
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(_cacheKey))
                return _cacheKey;

            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var queryString = context.HttpContext.Request.QueryString.ToString();

            if (_usernameRequired)
            {
                var username = context.HttpContext.User.FindFirst("username")?.Value;
                if (!string.IsNullOrEmpty(username))
                    return $"{controller}:{action}:{queryString}:user:{username}";
            }

            return $"{controller}:{action}:{queryString}";
        }
    }
}
