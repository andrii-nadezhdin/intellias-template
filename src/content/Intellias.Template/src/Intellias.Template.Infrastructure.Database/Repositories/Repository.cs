namespace Intellias.Template.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Intellias.Template.Domain.Abstractions;
    using Database.Contexts;
#if SQLServer
    using Microsoft.EntityFrameworkCore;
#endif

    public class Repository<T> : IRepository<T> where T : class
    {
#if SQLServer
        private readonly ApplicationDbContext dbContext;

        public Repository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

        public virtual async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate) =>
            await this.dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync()
                .ConfigureAwait(false);

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate) =>
            await this.dbContext.Set<T>()
                .Where(predicate)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

        public virtual async Task<T> GetByIdAsync(Guid id) =>
            await this.dbContext.Set<T>()
                .FindAsync(id)
                .ConfigureAwait(false);

        public async Task<T> AddAsync(T entity)
        {
            await this.dbContext.Set<T>().AddAsync(entity).ConfigureAwait(false);
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await this.GetByIdAsync(id).ConfigureAwait(false);
            this.dbContext.Set<T>().Remove(entity);
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await this.dbContext
                .Set<T>()
                .ToListAsync()
                .ConfigureAwait(false);
#else
        public virtual async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();

        public virtual async Task<T> GetByIdAsync(Guid id) => throw new NotImplementedException();

        public async Task<T> AddAsync(T entity) => throw new NotImplementedException();

        public async Task UpdateAsync(T entity) => throw new NotImplementedException();

        public async Task DeleteAsync(Guid id) => throw new NotImplementedException();

        public async Task<IReadOnlyList<T>> GetAllAsync() => throw new NotImplementedException();
#endif
    }
}
