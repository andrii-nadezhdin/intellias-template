namespace Intellias.Template.Api.Extensions
{
    using System.Linq;
    using Intellias.Template.Api.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.SwaggerGen;

    internal static partial class SwaggerExtensions
    {
        public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder application) =>
            application.UseSwaggerUI(
                options =>
                {
                    // Set the Swagger UI browser document title.
                    options.DocumentTitle = AssemblyInformation.Current.Product;
                    // Set the Swagger UI to render at '/'.
                    options.RoutePrefix = string.Empty;

                    options.DisplayOperationId();
                    options.DisplayRequestDuration();

                    var provider = application.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var apiVersionDescription in provider
                        .ApiVersionDescriptions
                        .OrderByDescending(x => x.ApiVersion))
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                            $"Version {apiVersionDescription.ApiVersion}");
                    }

                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Version 1");
                });

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services) =>
            services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
    }
}
