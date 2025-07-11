namespace Intellias.Template.Api.Middlewares
{
    using System.Net;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Intellias.Template.Api.Responses;
    using Intellias.Template.Application.Exceptions;
    using FluentValidation;
    using Microsoft.Extensions.Logging;

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                // TODO: can be improved to use
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage?view=aspnetcore-6.0
                this.logger.LogError(error, error.Message);
                var (apiError, statusCode) = MapToErrorAndStatusCode(error);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)statusCode;
                var result = JsonSerializer.Serialize(apiError, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                await response.WriteAsync(result).ConfigureAwait(false);
            }
        }

        private static (ApiError ApiError, HttpStatusCode StatusCode) MapToErrorAndStatusCode(Exception error)
        {
            HttpStatusCode statusCode;
            switch (error)
            {
                case ApplicationException:
                    statusCode = HttpStatusCode.BadRequest;
                    return (new ApiError((int)statusCode), statusCode);
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    return (new ApiError((int)statusCode), statusCode);
                case ValidationException e:
                    statusCode = HttpStatusCode.BadRequest;
                    var errors = e.Errors.Select(r => r.ErrorMessage).ToArray();
                    return (new ApiError((int)statusCode, string.Join("\n", errors)), statusCode);
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    return (new ApiError((int)statusCode), statusCode);
            }
        }
    }
}
