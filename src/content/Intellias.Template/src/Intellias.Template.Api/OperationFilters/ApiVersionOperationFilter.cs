namespace Intellias.Template.Api.OperationFilters
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// An Open API operation filter used to document the implicit API version parameter.
    /// </summary>
    public class ApiVersionOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters is null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions
                    .First(x => string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

                if (parameter.Description is null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default is null && description.DefaultValue is not null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
