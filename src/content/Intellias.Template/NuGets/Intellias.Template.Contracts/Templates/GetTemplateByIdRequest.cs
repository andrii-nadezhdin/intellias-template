namespace Intellias.Template.Contracts.Templates
{
    using MediatR;

    public class GetTemplateByIdRequest : IRequest<TemplateDTO>
    {
        public GetTemplateByIdRequest(Guid id) => this.Id = id;

        public Guid Id { get; set; }
    }
}
