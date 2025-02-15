using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest, OperationRequestId>
    {
        public Task<List<OperationRequest>> GetAllOperationRequest();
        Task<OperationRequest> GetByIdEagerAsync(OperationRequestId id);
    }
}