namespace Intellias.Template.Api.OperationFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ClaimsOperationFilter : IOperationFilter
    {
        private static readonly OpenApiSecurityScheme OAuth2OpenApiSecurityScheme = new()
        {
            Reference = new OpenApiReference
            {
                Id = "oauth2",
                Type = ReferenceType.SecurityScheme,
            },
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var list = (from x in GetPolicyRequirements(context.ApiDescription.ActionDescriptor.FilterDescriptors).OfType<ClaimsAuthorizationRequirement>()
                        select x.ClaimType).ToList();
            if (list.Any())
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            OAuth2OpenApiSecurityScheme,
                            list
                        }
                    }
                };
            }
        }

        private static IList<IAuthorizationRequirement> GetPolicyRequirements(IList<Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor> filterDescriptors)
        {
            var list = new List<IAuthorizationRequirement>();
            foreach (var filterDescriptor in filterDescriptors)
            {
                if (filterDescriptor.Filter is AllowAnonymousFilter)
                {
                    break;
                }

                if (filterDescriptor.Filter is AuthorizeFilter authorizeFilter && authorizeFilter.Policy != null)
                {
                    list.AddRange(authorizeFilter.Policy.Requirements);
                }
            }

            return list;
        }
    }
}
