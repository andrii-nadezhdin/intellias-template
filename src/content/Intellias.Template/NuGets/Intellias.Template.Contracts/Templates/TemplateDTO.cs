namespace Intellias.Template.Contracts.Templates
{
    public class TemplateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TemplateTypeId { get; set; }
    }
}
