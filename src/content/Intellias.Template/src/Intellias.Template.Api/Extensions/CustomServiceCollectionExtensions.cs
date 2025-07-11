namespace Intellias.Template.Api.Extensions
{
    using System;
#if CORS
    using Intellias.Template.Api.Constants;
#endif
    using Intellias.Template.Api.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    internal static class CustomServiceCollectionExtensions
    {
        // TODO: replace with single implementation and switch between cache options in setted in config
#if DistributedCacheInMemory
        public static IServiceCollection AddCustomCaching(this IServiceCollection services) =>
#elif DistributedCacheRedis
        public static IServiceCollection AddCustomCaching(
            this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration) =>
#endif
#if DistributedCacheInMemory
                services.AddDistributedMemoryCache();
#elif DistributedCacheRedis
                services.AddStackExchangeRedisCache(
                    options => options.ConfigurationOptions = configuration
                        .GetSection(nameof(ApplicationOptions.Redis))
                        .Get<RedisOptions>()
                        .ConfigurationOptions);
#endif

#if CORS
        public static IServiceCollection AddCustomCors(this IServiceCollection services) =>
            services.AddCors(
                options =>
                    options.AddPolicy(
                        CorsPolicyName.AllowAny,
                        x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()));

#endif

        public static IServiceCollection AddCustomOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<ApplicationOptions>().Bind(configuration).ValidateDataAnnotations();
            services.AddSingleton((IServiceProvider x) => x.GetRequiredService<IOptions<ApplicationOptions>>().Value);

            services.AddOptions<ForwardedHeadersOptions>().Bind(configuration).ValidateDataAnnotations();
            services.AddSingleton((IServiceProvider x) => x.GetRequiredService<IOptions<ForwardedHeadersOptions>>().Value).Configure<ForwardedHeadersOptions>(
                options =>
                {
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });

            services.AddOptions<KestrelServerOptions>().Bind(configuration.GetSection(nameof(ApplicationOptions.Kestrel))).ValidateDataAnnotations();
            services.AddSingleton((IServiceProvider x) => x.GetRequiredService<IOptions<KestrelServerOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddCustomStrictTransportSecurity(this IServiceCollection services) =>
            services
                .AddHsts(
                    options =>
                    {
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(365);
                        options.Preload = true;
                    });

        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services) =>
            services
                .AddApiVersioning(
                    options =>
                    {
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.ReportApiVersions = true;
                    })
                .AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV"); // Version format: 'v'major[.minor][-status]
    }
}
