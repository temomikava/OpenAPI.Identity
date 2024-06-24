
namespace SharedKernel
{
    public interface IRepository<TEntity, in TId> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(TId id);
    }
}
