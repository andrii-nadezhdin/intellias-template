namespace Intellias.Template.Domain.Abstractions
{
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
