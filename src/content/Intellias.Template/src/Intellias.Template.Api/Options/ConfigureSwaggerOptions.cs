namespace Intellias.Template.Api.Options
{
    using System.IO;
    using Intellias.Template.Api.OperationFilters;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            options.DescribeAllParametersInCamelCase();
            options.EnableAnnotations();

            // Add the XML comment file for this assembly, so its contents can be displayed.
            options.IncludeXmlComments(Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml"));

            options.OperationFilter<ApiVersionOperationFilter>();
            options.OperationFilter<ClaimsOperationFilter>();

            foreach (var apiVersionDescription in this.provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo()
                {
                    Title = AssemblyInformation.Current.Product,
                    Description = apiVersionDescription.IsDeprecated ?
                        $"This API version has been deprecated." : string.Empty,
                    Version = apiVersionDescription.ApiVersion.ToString(),
                };
                options.SwaggerDoc(apiVersionDescription.GroupName, info);
            }
        }
    }
}
