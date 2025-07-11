namespace Intellias.Template.Api.IntegrationTest
{
    using System.Net.Http;
    using Intellias.Template.Api.Options;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Serilog;
    using Serilog.Events;
    using Xunit.Abstractions;

    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        public CustomWebApplicationFactory(ITestOutputHelper testOutputHelper)
        {
            this.ClientOptions.AllowAutoRedirect = false;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.TestOutput(testOutputHelper, LogEventLevel.Verbose)
                .CreateLogger();
        }

        public ApplicationOptions ApplicationOptions { get; private set; } = default!;

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                this.ApplicationOptions = serviceProvider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
            }

            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder
                .UseEnvironment(Constants.EnvironmentName.Test)
                .ConfigureServices(this.ConfigureServices);


        protected virtual void ConfigureServices(IServiceCollection services) =>
#if DistributedCacheRedis
            services
                .AddDistributedMemoryCache();
#else
            return null;
#endif


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}
