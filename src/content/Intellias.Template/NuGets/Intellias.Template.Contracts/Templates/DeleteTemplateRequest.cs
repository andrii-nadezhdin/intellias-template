namespace Intellias.Template.Contracts.Templates
{
    using MediatR;

    public class DeleteTemplateRequest : IRequest<Guid>
    {
        public DeleteTemplateRequest(Guid id) => this.Id = id;

        public Guid Id { get; set; }
    }
}
