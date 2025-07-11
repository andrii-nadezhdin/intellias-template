namespace Intellias.Template.Api.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using Serilog.Events;

    internal static partial class SerilogExtensions
    {
        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder application) =>
            application.UseSerilogRequestLogging(
                options =>
                {
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        var request = httpContext.Request;
                        var response = httpContext.Response;
                        var endpoint = httpContext.GetEndpoint();

                        diagnosticContext.Set("Host", request.Host);
                        diagnosticContext.Set("Protocol", request.Protocol);
                        diagnosticContext.Set("Scheme", request.Scheme);

                        if (request.QueryString.HasValue)
                        {
                            diagnosticContext.Set("QueryString", request.QueryString.Value);
                        }

                        if (endpoint is not null)
                        {
                            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
                        }

                        diagnosticContext.Set("ContentType", response.ContentType);
                    };
                    options.GetLevel = GetLevel;

                    static LogEventLevel GetLevel(HttpContext httpContext, double elapsedMilliseconds, Exception exception)
                    {
                        if (exception == null && httpContext.Response.StatusCode <= 499)
                        {
                            return IsHealthCheckEndpoint(httpContext) ? LogEventLevel.Verbose : LogEventLevel.Information;
                        }

                        return LogEventLevel.Error;
                    }

                    static bool IsHealthCheckEndpoint(HttpContext httpContext) => httpContext.GetEndpoint()?.DisplayName == "Health checks";
                });
    }
}
