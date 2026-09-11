using System.Net;
using System.Text.Json;
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using FluentValidation;

namespace Aryholding.Tms.TicketManagement.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ErrorResponse
            {
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Doğrulama hatası oluştu.";
                    response.ErrorCode = "VALIDATION_ERROR";
                    response.Errors = validationEx.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    break;

                case ValidationFailedException validationFailedEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Doğrulama hatası oluştu.";
                    response.ErrorCode = "VALIDATION_FAILED";
                    response.Errors = validationFailedEx.GetErrorDictionary();
                    break;

                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = notFoundEx.Message;
                    response.ErrorCode = "NOT_FOUND";
                    response.Details = new { 
                        ResourceName = notFoundEx.ResourceName, 
                        ResourceKey = notFoundEx.ResourceKey 
                    };
                    break;

                case UnauthorizedException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = unauthorizedEx.Message;
                    response.ErrorCode = "UNAUTHORIZED";
                    break;

                case BusinessException businessEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = businessEx.Message;
                    response.ErrorCode = businessEx.ErrorCode;
                    response.Details = businessEx.Details;
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = argEx.Message;
                    response.ErrorCode = "INVALID_ARGUMENT";
                    break;

                case InvalidOperationException invalidOpEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = invalidOpEx.Message;
                    response.ErrorCode = "INVALID_OPERATION";
                    break;

                case TimeoutException timeoutEx:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Message = "İşlem zaman aşımına uğradı.";
                    response.ErrorCode = "TIMEOUT";
                    break;

                case TaskCanceledException taskCanceledEx when taskCanceledEx.InnerException is TimeoutException:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Message = "İstek zaman aşımına uğradı.";
                    response.ErrorCode = "REQUEST_TIMEOUT";
                    break;

                case TaskCanceledException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "İşlem iptal edildi.";
                    response.ErrorCode = "OPERATION_CANCELLED";
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException concurrencyEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "Veri güncellenirken bir çakışma oluştu. Lütfen tekrar deneyin.";
                    response.ErrorCode = "CONCURRENCY_CONFLICT";
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Veri kaydedilirken bir hata oluştu.";
                    response.ErrorCode = "DATABASE_UPDATE_ERROR";
                    
                    if (IsDevelopmentEnvironment())
                    {
                        response.Details = new
                        {
                            InnerException = dbUpdateEx.InnerException?.Message
                        };
                    }
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "İç sunucu hatası oluştu.";
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    
                    if (IsDevelopmentEnvironment())
                    {
                        response.Details = new
                        {
                            ExceptionType = exception.GetType().Name,
                            StackTrace = exception.StackTrace,
                            InnerException = exception.InnerException?.Message
                        };
                    }
                    break;
            }

            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private bool IsDevelopmentEnvironment()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return environment?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
