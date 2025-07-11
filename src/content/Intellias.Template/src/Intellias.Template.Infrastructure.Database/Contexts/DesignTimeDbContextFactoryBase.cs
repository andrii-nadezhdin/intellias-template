namespace Intellias.Template.Infrastructure.Database.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Options;

    public abstract class DesignTimeDbContextFactoryBase<TContext> :
        IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly string aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        public TContext CreateDbContext(string[] args) =>
            this.Create(Directory.GetCurrentDirectory(), this.aspnetcoreEnvironment);

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        public TContext Create() => this.Create(AppContext.BaseDirectory, this.aspnetcoreEnvironment);

        private TContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetSection(SQLServerOptions.Name)
                .Get<SQLServerOptions>()
                .ConnectionString;

            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException("Could not find a connection string named 'Default'.");
            }
            return this.Create(connstr);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var options = optionsBuilder.Options;
            return this.CreateNewInstance(options);
        }
    }
}
