#if CORS
using Intellias.Template.Api.Constants;
using Intellias.Template.Api.Extensions;
using Intellias.Template.Api.Middlewares;
#endif
using System.Reflection;
using Intellias.Template.Api;
using Intellias.Template.Api.Infrastructure;
using Intellias.Template.Api.Options;
using Intellias.Template.Application.Extensions;
using Intellias.Template.Infrastructure.Database.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

// Will be replaced by normal logger when services are initialized
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateBootstrapLogger();

Log.Information("Initializing.");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddCustomJsonOptions(builder.Environment)
    .AddCustomMvcOptions(builder.Configuration);

builder.Services.AddApplication()
    .AddDatabaseContext(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCustomSwagger();
builder.AddSerilog();

#if DistributedCacheRedis
builder.Services.AddCustomCaching(builder.Environment, builder.Configuration);
#elif DistributedCacheInMemory
builder.Services.AddCustomCaching()
#endif

#if CORS
builder.Services.AddCustomCors();
#endif

builder.Services.AddCustomOptions(builder.Configuration);
    
#if ResponseCaching
builder.Services.AddResponseCaching();
#endif

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomApiVersioning();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCustomStrictTransportSecurity();
}

builder.Host.ConfigureHostConfiguration(configurationBuilder =>
        configurationBuilder.AddEnvironmentVariables(prefix: "DOTNET_"))
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        if (hostingContext.HostingEnvironment.IsDevelopment() &&
            !string.IsNullOrEmpty(hostingContext.HostingEnvironment.EnvironmentName))
        {
            config.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: false);
        }
    })
    .UseDefaultServiceProvider(
        (context, options) =>
        {
            var isDevelopment = context.HostingEnvironment.IsDevelopment();
            options.ValidateScopes = isDevelopment;
            options.ValidateOnBuild = isDevelopment;
        });

builder.WebHost.UseKestrel((context, options) =>
{
    options.AddServerHeader = false;
    options.Configure(context.Configuration.GetSection(nameof(ApplicationOptions.Kestrel)), reloadOnChange: false);
});

#if IIS
builder.WebHost.UseIIS();
#endif

builder.Host.UseConsoleLifetime();

var app = builder.Build();

var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
hostEnvironment.ApplicationName = AssemblyInformation.Current.Product;

Log.Information(
    "WebApplicationBuilder.Build() is completed for {Application} in {Environment} mode.",
    hostEnvironment.ApplicationName,
    hostEnvironment.EnvironmentName);

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app.UseRouting();

app.UseCustomSerilogRequestLogging();

#if CORS
app.UseCors(CorsPolicyName.AllowAny);
#endif

#if ResponseCaching
app.UseResponseCaching();
#endif

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseEndpoints(
    routeBuilder =>
    {
#if CORS
        routeBuilder
            .MapControllers().RequireCors(CorsPolicyName.AllowAny);
#else
        builder.MapControllers();
#endif
#if CORS
        routeBuilder
            .MapHealthChecks("/status")
            .RequireCors(CorsPolicyName.AllowAny);
        routeBuilder
            .MapHealthChecks("/status/self", new HealthCheckOptions() {Predicate = _ => false})
            .RequireCors(CorsPolicyName.AllowAny);
#else
        builder.MapHealthChecks("/status");
        builder.MapHealthChecks("/status/self", new HealthCheckOptions() { Predicate = _ => false });
#endif
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

Log.Information(
    "Started {Application} in {Environment} mode.",
    hostEnvironment.ApplicationName,
    hostEnvironment.EnvironmentName);

app.Run();

Log.Information(
    "Stopped {Application} in {Environment} mode.",
    hostEnvironment.ApplicationName,
    hostEnvironment.EnvironmentName);

// Make the implicit Program class public so test projects can access it
public partial class Program { }
