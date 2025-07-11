namespace Intellias.Template.Api.Infrastructure;

using System.Diagnostics.CodeAnalysis;
#if ApplicationInsights
using Microsoft.ApplicationInsights.Extensibility;
#endif
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;

/// <summary>
/// Class that provides extension methods to add logging providers to <see cref="WebApplicationBuilder"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class SerilogExtensions
{
    /// <summary>
    /// Setups and adds Serilog logging framework.
    /// </summary>
    /// <param name="builder">The instance of <see cref="WebApplicationBuilder"/>.</param>
    /// <returns>>The instance of <see cref="WebApplicationBuilder"/>.</returns>
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor(); // need for Enrich.WithCorrelationId

        var logger = new LoggerConfiguration()
            .SetLoggingLevel(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithSpan()
            .Enrich.WithCorrelationId()
            .WriteTo.Conditional(
                _ => builder.Environment.IsDevelopment(),
                x => x
                    .Console() // console call is slow: https://weblog.west-wind.com/posts/2018/Dec/31/Dont-let-ASPNET-Core-Default-Console-Logging-Slow-your-App-down
                    .WriteTo.Debug())
#if Splunk
            .SetupSplunk(builder.Configuration)
#endif
#if ApplicationInsights
            .SetupApplicationInsights(builder)
#endif
            .CreateLogger();

        Log.Logger = logger;
        builder.Host.UseSerilog(logger);

        return builder;
    }

#if Splunk
    private static LoggerConfiguration SetupSplunk(this LoggerConfiguration loggerConfiguration, ConfigurationManager configurationManager)
    {
        // Write to HTTP event collector
        var eventHost = configurationManager["Serilog:Splunk:EventCollector:Host"];
        var token = configurationManager["Serilog:Splunk:EventCollector:EventCollectorToken"];

        if (string.IsNullOrWhiteSpace(eventHost) || string.IsNullOrWhiteSpace(token))
        {
            return loggerConfiguration;
        }

        return loggerConfiguration.WriteTo.EventCollector(eventHost, token);
    }
#endif

#if ApplicationInsights
    private static LoggerConfiguration SetupApplicationInsights(this LoggerConfiguration loggerConfiguration, WebApplicationBuilder builder)
    {
        var instrumentationKey = builder.Configuration["Serilog:ApplicationInsights:InstrumentationKey"];

        if (string.IsNullOrWhiteSpace(instrumentationKey))
        {
            return loggerConfiguration;
        }

        // To allow using of deprecated TelemetryConfiguration.Active: https://github.com/serilog/serilog-sinks-applicationinsights/issues/156
        _ = builder.Services.AddApplicationInsightsTelemetry(opt =>
          {
              opt.EnableActiveTelemetryConfigurationSetup = true;
              opt.InstrumentationKey = instrumentationKey;
          });

#pragma warning disable CS0618
        return loggerConfiguration.WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces);
#pragma warning restore CS0618
    }
#endif

    private static LoggerConfiguration SetLoggingLevel(this LoggerConfiguration loggerConfiguration, ConfigurationManager configurationManager)
    {
        var defaultLogLevelString = configurationManager["Logging:LogLevel:Default"];

        if (!Enum.TryParse<LogLevel>(defaultLogLevelString, out var defaultLogLevel))
        {
            return loggerConfiguration;
        }

        var serilogEventLevel = LogEventLevel.Information;

        switch (defaultLogLevel)
        {
            case LogLevel.Trace:
                serilogEventLevel = LogEventLevel.Verbose;
                break;
            case LogLevel.Debug:
                serilogEventLevel = LogEventLevel.Debug;
                break;
            case LogLevel.Information:
                serilogEventLevel = LogEventLevel.Information;
                break;
            case LogLevel.Warning:
                serilogEventLevel = LogEventLevel.Warning;
                break;
            case LogLevel.Error:
                serilogEventLevel = LogEventLevel.Error;
                break;
            case LogLevel.Critical:
                serilogEventLevel = LogEventLevel.Fatal;
                break;
            case LogLevel.None:
                break;
            default:
                serilogEventLevel = LogEventLevel.Information;
                break;
        }

        return loggerConfiguration.MinimumLevel.Is(serilogEventLevel);
    }
}
