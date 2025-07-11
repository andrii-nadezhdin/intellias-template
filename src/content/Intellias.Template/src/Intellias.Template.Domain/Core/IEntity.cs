namespace Intellias.Template.Domain.Core
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
