namespace Intellias.Template.Infrastructure.Database.Extensions
{
    using Contexts;
    using Domain.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Persistence.Repositories;

    public static class DatabaseContextExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration) =>
            services
#if SQLServer
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetSection(SQLServerOptions.Name).Get<SQLServerOptions>().ConnectionString!,
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)))
#endif
                .AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }
}
