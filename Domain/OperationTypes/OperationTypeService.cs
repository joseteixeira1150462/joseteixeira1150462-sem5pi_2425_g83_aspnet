using System;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationTypeRepository _repository;
        
        public OperationTypeService(
            IUnitOfWork unitOfWork,
            IOperationTypeRepository repository
        ) {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<OperationTypeDto> GetByIdAsync(OperationTypeId id)
        {
            var operationType = await _repository.GetByIdEagerAsync(id);

            if (operationType == null)
                return null;

            return OperationTypeMapper.DomainToDto(operationType);
        }

        public async Task<List<OperationTypeDto>> GetAllAsync()
        {
            var operationTypes = await _repository.GetAllAsync();

            return operationTypes.Select(OperationTypeMapper.DomainToDto).ToList();
        }

        public async Task<OperationTypeDto> AddAsync(CreatingOperationTypeDto dto)
        {
            OperationTypeId id = new OperationTypeId(Guid.NewGuid());
            OperationType operationType = OperationTypeMapper.CreatingDtoToDomain(id, dto);

            await _repository.AddAsync(operationType);

            await _unitOfWork.CommitAsync();

            return OperationTypeMapper.DomainToDto(operationType);
        }

        public async Task<OperationTypeDto> DeactivateAsync(string id)
        {
            OperationTypeId otid = new OperationTypeId(id);
            OperationType operationType = await _repository.GetByIdAsync(otid);

            if (operationType == null)
                return null;

            operationType.MarkAsInactive();

            await _unitOfWork.CommitAsync();

            return OperationTypeMapper.DomainToDto(operationType);
        }

        public async Task<List<OperationTypeDto>> QueryAsync(IQueryCollection query)
        {
            List<OperationType> operationTypes = await _repository.ApplyQuery(query);

            return operationTypes.Select(OperationTypeMapper.DomainToDto).ToList();
        }
    }
}