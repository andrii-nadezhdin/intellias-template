namespace Intellias.Template.Infrastructure.Database.Contexts
{
    using Microsoft.EntityFrameworkCore;

    public class DbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
    {
        protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options) => new(options);
    }
}
