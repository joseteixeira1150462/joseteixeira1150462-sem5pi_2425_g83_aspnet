using System.Threading.Tasks;

namespace HealthCare.Domain.Shared
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}