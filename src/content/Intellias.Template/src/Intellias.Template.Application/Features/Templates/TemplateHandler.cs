namespace Intellias.Template.Application.Features.Templates
{
    using AutoMapper;
    using Intellias.Template.Contracts.Templates;
    using Intellias.Template.Domain.Abstractions;
    using Intellias.Template.Domain.Entities;
    using MediatR;

    public class TemplateHandler :
        IRequestHandler<CreateTemplateRequest, Guid>,
        IRequestHandler<DeleteTemplateRequest, Guid>,
        IRequestHandler<UpdateTemplateRequest, Guid>,
        IRequestHandler<GetTemplateByIdRequest, TemplateDTO>,
        IRequestHandler<GetAllTemplatesRequest, TemplateDTO[]>
    {
        private readonly IRepository<Template> repository;
        private readonly IMapper mapper;

        public TemplateHandler(IRepository<Template> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTemplateRequest request, CancellationToken cancellationToken)
        {
            var template = this.mapper.Map<Template>(request);
            await this.repository.AddAsync(template).ConfigureAwait(false);
            return template.Id;
        }

        public async Task<Guid> Handle(DeleteTemplateRequest request, CancellationToken cancellationToken)
        {
            await this.repository.DeleteAsync(request.Id).ConfigureAwait(false);
            return request.Id;
        }

        public async Task<Guid> Handle(UpdateTemplateRequest request, CancellationToken cancellationToken)
        {
            var template = await this.repository.GetByIdAsync(request.Id).ConfigureAwait(false);
            template.SetName(request.Name);
            template.SetDescription(request.Description);
            template.SetTemplateTypeId(request.TemplateTypeId);
            await this.repository.UpdateAsync(template).ConfigureAwait(false);
            return request.Id;
        }

        public async Task<TemplateDTO> Handle(GetTemplateByIdRequest request, CancellationToken cancellationToken)
        {
            var template = await this.repository.GetByIdAsync(request.Id).ConfigureAwait(false);
            return this.mapper.Map<TemplateDTO>(template);
        }

        public async Task<TemplateDTO[]> Handle(GetAllTemplatesRequest request, CancellationToken cancellationToken)
        {
            var templates = await this.repository.GetAllAsync().ConfigureAwait(false);
            return this.mapper.Map<TemplateDTO[]>(templates.ToArray());
        }
    }
}
