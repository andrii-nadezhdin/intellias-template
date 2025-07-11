namespace Intellias.Template.Contracts.Templates
{
    using MediatR;

    public class UpdateTemplateRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TemplateTypeId { get; set; }
    }
}
