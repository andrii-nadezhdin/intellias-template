namespace Intellias.Template.Api.Extensions
{
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Intellias.Template.Api.Options;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    internal static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomJsonOptions(
            this IMvcBuilder builder,
            IWebHostEnvironment webHostEnvironment) =>
            builder.AddJsonOptions(
                options =>
                {
                    var jsonSerializerOptions = options.JsonSerializerOptions;
                    if (webHostEnvironment.IsDevelopment() ||
                        webHostEnvironment.IsEnvironment(Constants.EnvironmentName.Test))
                    {
                        // Pretty print the JSON in development for easier debugging.
                        jsonSerializerOptions.WriteIndented = true;
                    }

                    jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    jsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

        public static IMvcBuilder AddCustomMvcOptions(this IMvcBuilder builder, IConfiguration configuration) =>
            builder.AddMvcOptions(
                options =>
                {
                    var cacheProfileOptions = configuration
                        .GetSection(nameof(ApplicationOptions.CacheProfiles))
                        .Get<CacheProfileOptions>();
                    if (cacheProfileOptions != null)
                    {
                        foreach (var keyValuePair in cacheProfileOptions)
                        {
                            options.CacheProfiles.Add(keyValuePair);
                        }
                    }

                    options.OutputFormatters.RemoveType<StringOutputFormatter>();
                    options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                    options.ReturnHttpNotAcceptable = true;
                });

        /// <summary>
        /// Gets the JSON patch input formatter. The <see cref="JsonPatchDocument{T}"/> does not support the new
        /// System.Text.Json API's for de-serialization. You must use Newtonsoft.Json instead
        /// (See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.0#jsonpatch-addnewtonsoftjson-and-systemtextjson).
        /// </summary>
        /// <returns>The JSON patch input formatter using Newtonsoft.Json.</returns>
        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddMvcCore()
                .AddNewtonsoftJson()
                .Services;
            var serviceProvider = services.BuildServiceProvider();
            var mvcOptions = serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
            return mvcOptions.InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
