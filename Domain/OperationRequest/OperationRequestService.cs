using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Shared.TimeSlot;
using System.Collections.Generic;
using HealthCare.Domain.Patients;
using HealthCare.Domain.Staffs;
using HealthCare.Domain.OperationTypes;

namespace HealthCare.Domain.OperationRequests
{
    public class OperationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationRequestRepository _repo;
        private readonly IConfiguration _configuration;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _configuration = configuration;
        }


        public async Task<List<OperationRequestDto>> GetAllAOperationRequest()
        {

            var list = await _repo.GetAllOperationRequest();

            // Converte cada OperationRequest em um OperationRequestDto
            return list.Select(req => new OperationRequestDto
            {
                Id = req.Id.AsGuid(),
                PatientId = req.PatientId.Value,
                DoctorId = req.DoctorId.Value,
                OperationTypeId = req.OperationTypeId.Value,
                Priority = req.Priority.ToString(),
                DeadlineDate = req.DeadlineDate
            }).ToList();
        }

        public async Task<OperationRequestDto> GetByIdAsync(OperationRequestId id)
        {
            OperationRequest OperationRequest = await _repo.GetByIdEagerAsync(id);

            if (OperationRequest == null)
                return null;

            // Retorna o OperationRequestDto com todos os campos mapeados
            return new OperationRequestDto
            {
                Id = OperationRequest.Id.AsGuid(),
                PatientId = OperationRequest.PatientId.Value,
                DoctorId = OperationRequest.DoctorId.Value,
                OperationTypeId = OperationRequest.OperationTypeId.Value,
                Priority = OperationRequest.Priority.ToString(),
                DeadlineDate = OperationRequest.DeadlineDate
            };
        }

        // Método para adicionar um novo OperationRequest
        public async Task<OperationRequestDto> AddAsync(CreateOperationRequestDto dto)
        {
            var operationRequest = new OperationRequest(
                new PatientId(dto.PatientId),
                new StaffId(dto.DoctorId),
                new OperationTypeId(dto.OperationTypeId),
                (OperationRequestPriority)Enum.Parse(typeof(OperationRequestPriority), dto.OperationRequestPriority),
                dto.DeadlineDate
            );

            await _repo.AddAsync(operationRequest);
            await _unitOfWork.CommitAsync();

            return new OperationRequestDto
            {
                Id = operationRequest.Id.AsGuid(),
                PatientId = operationRequest.PatientId.Value,
                DoctorId = operationRequest.DoctorId.Value,
                OperationTypeId = operationRequest.OperationTypeId.Value,
                Priority = operationRequest.Priority.ToString(),
                DeadlineDate = operationRequest.DeadlineDate
            };
        }

        public async Task<OperationRequestDto> UpdateAsync(OperationRequestId id, EditOperationRequestDto dto)
        {
            var operationRequest = await _repo.GetByIdAsync(id);
            if (operationRequest == null) return null;

            if (dto.PatientId != null) {
                operationRequest.ChangePatientId(dto.PatientId);
            }
            if (dto.DoctorId != null) {
                operationRequest.ChangeDoctorId(dto.DoctorId);
            }
            if (dto.OperationTypeId != null) {
                operationRequest.ChangeOperationTypeId(dto.OperationTypeId);
            }
            if (dto.Priority != null) {
                operationRequest.ChangePriority(dto.Priority);
            }
            if (dto.DeadlineDate != null) {
                operationRequest.ChangeDeadlineDate(dto.DeadlineDate);
            }

            await _unitOfWork.CommitAsync();

            return new OperationRequestDto
            {
                Id = operationRequest.Id.AsGuid(),
                PatientId = operationRequest.PatientId.Value,
                DoctorId = operationRequest.DoctorId.Value,
                OperationTypeId = operationRequest.OperationTypeId.Value,
                Priority = operationRequest.Priority.ToString(),
                DeadlineDate = operationRequest.DeadlineDate
            };
        }

        public async Task<bool> DeleteAsync(OperationRequestId id)
        {
            var operationRequest = await _repo.GetByIdAsync(id);
            if (operationRequest == null) return false;

            _repo.Remove(operationRequest);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}