using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Domain.OperationTypes
{
    public interface IOperationTypeRepository : IRepository<OperationType, OperationTypeId>
    {
        public Task<OperationType> GetByIdEagerAsync(OperationTypeId id);
        public Task<List<OperationType>> ApplyQuery(IQueryCollection query);
    }
}