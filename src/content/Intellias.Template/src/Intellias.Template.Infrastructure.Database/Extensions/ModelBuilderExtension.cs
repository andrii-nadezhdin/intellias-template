namespace Intellias.Template.Infrastructure.Database.Extensions
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    internal static class ModelBuilderExtension
    {
        internal static void BuildApplicationModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Template>(e =>
            {
                e.HasKey(e => e.Id).IsClustered();
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.HasIndex(e => e.Name).IsUnique();
                e.Property(e => e.Name).IsRequired();
                e.HasOne(e => e.TemplateType);

                e.HasData(
                    new Template(Guid.NewGuid(), "Notification Service", null, TemplateTypes.TemplateTypeSystemApiId),
                    new Template(Guid.NewGuid(), "Email Service", null, TemplateTypes.TemplateTypeSystemApiId),
                    new Template(Guid.NewGuid(), "Orders Service", null, TemplateTypes.TemplateTypeProcessApiId),
                    new Template(Guid.NewGuid(), "Orders BFF", null, TemplateTypes.TemplateTypePresentationApiId)
                );
            });

            modelBuilder.Entity<TemplateType>(e =>
            {
                e.HasKey(e => e.Id).IsClustered();
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.Name).IsRequired();

                e.HasData(
                    new TemplateType(TemplateTypes.TemplateTypeSystemApiId, "System API"),
                    new TemplateType(TemplateTypes.TemplateTypeProcessApiId, "Process API"),
                    new TemplateType(TemplateTypes.TemplateTypePresentationApiId, "Presentation API")
                );
            });
        }
    }
}
