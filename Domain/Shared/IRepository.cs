using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Domain.Shared
{
    public interface IRepository<TEntity, TEntityId>
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TEntityId id);
        Task<List<TEntity>> GetByIdsAsync(List<TEntityId> ids);
        Task<TEntity> AddAsync(TEntity obj);
        Task<TEntity> UpdateAsync(TEntity obj);
        void Remove(TEntity obj);
    }
}