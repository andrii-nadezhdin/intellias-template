namespace Intellias.Template.Contracts.Templates
{
    using MediatR;

    //TODO: Add pagination
    public class GetAllTemplatesRequest : IRequest<TemplateDTO[]>
    {
    }
}
