namespace Intellias.Template.Contracts.Templates
{
    using MediatR;

    public class CreateTemplateRequest : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TemplateTypeId { get; set; }
    }
}
