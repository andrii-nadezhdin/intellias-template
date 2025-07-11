namespace Intellias.Template.Application.Extensions
{
    using System.Reflection;
    using FluentValidation;
    using Intellias.Template.Application.Core.Behaviours;
    using Intellias.Template.Application.Mappings;
    using Intellias.Template.Contracts.Templates.Validators;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return services
                .AddAutoMapper(cfg => cfg.AddProfile<ApplicationMappingProfile>())
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
                .AddValidatorsFromAssemblyContaining(typeof(CreateTemplateRequestValidator))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
