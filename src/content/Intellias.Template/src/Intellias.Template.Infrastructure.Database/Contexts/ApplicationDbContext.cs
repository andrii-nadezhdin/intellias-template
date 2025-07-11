namespace Intellias.Template.Infrastructure.Database.Contexts
{
    using Domain.Entities;
    using Extensions;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Template> Templates => this.Set<Template>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.BuildApplicationModel();
    }
}
