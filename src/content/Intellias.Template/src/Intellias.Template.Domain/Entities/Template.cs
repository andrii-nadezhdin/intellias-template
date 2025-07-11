namespace Intellias.Template.Domain.Entities
{
    using Intellias.Template.Domain.Core;

    public class Template : IEntity<Guid>
    {
        public Template(string name, string? description, Guid templateTypeId) :
            this(Guid.NewGuid(), name, description, templateTypeId)
        {
        }

        public Template(Guid id, string name, string? description, Guid templateTypeId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.TemplateTypeId = templateTypeId;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Guid TemplateTypeId { get; private set; }
        public TemplateType TemplateType { get; set; } = null!;

        public void SetName(string name) => this.Name = name ?? throw new ArgumentNullException(nameof(name));
        public void SetDescription(string description) => this.Description = description;
        public void SetTemplateTypeId(Guid templateTypeId) =>
            this.TemplateTypeId = templateTypeId == Guid.Empty ? throw new ArgumentException($"{nameof(templateTypeId)} shouldn't be empty") : templateTypeId;
    }
}
