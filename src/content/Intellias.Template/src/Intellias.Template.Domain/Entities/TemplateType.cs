namespace Intellias.Template.Domain.Entities
{
    using Intellias.Template.Domain.Core;

    public class TemplateType : IEntity<Guid>
    {
        public TemplateType(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
