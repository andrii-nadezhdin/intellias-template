namespace Intellias.Template.Application.Mappings
{
    using AutoMapper;
    using Intellias.Template.Contracts.Templates;
    using Intellias.Template.Domain.Entities;

    internal class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            this.CreateMap<Template, TemplateDTO>();
            this.CreateMap<CreateTemplateRequest, Template>();
        }
    }
}
