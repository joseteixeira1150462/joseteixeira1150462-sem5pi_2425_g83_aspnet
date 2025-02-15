using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Shared.TimeSlot;
using System.Collections.Generic;

namespace HealthCare.Domain.Staffs
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;
        private readonly IConfiguration _configuration;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _configuration = configuration;
        }


        public async Task<List<StaffDto>> GetAllAStaff()
        {

            var list = await _repo.GetAllStaff();

            // Converte cada Staff em um StaffDto
            return list.Select(staff => new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                Specialization = staff.Specialization.ToString(),
                StaffEmail = staff.Email.Value,
                Phone = staff.Phone.Value,
                LicenseNumber = staff.LicenseNumber.Number,
                AvailabilitySlots = staff.AvailabilitySlots
                    .Select(slot => new TimeSlotDto { Start = slot.Starting, End = slot.Ending })
                    .ToList()
            }).ToList();
        }

        public async Task<StaffDto> GetByIdAsync(StaffId id)
        {
            Staff staff = await _repo.GetByIdEagerAsync(id);

            if (staff == null)
                return null;

            // Mapeia os TimeSlots da entidade Staff para TimeSlotDto
            var availabilitySlotsDto = staff.AvailabilitySlots
                .Select(slot => new TimeSlotDto { Start = slot.Starting, End = slot.Ending })
                .ToList();

            // Retorna o StaffDto com todos os campos mapeados
            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Specialization = staff.Specialization.ToString(),
                StaffEmail = staff.Email.Value,
                Phone = staff.Phone.Value,
                AvailabilitySlots = availabilitySlotsDto
            };
        }

        // Método para adicionar um novo Staff
        public async Task<StaffDto> AddAsync(CreatingStaffDto dto)
        {
            var staffPhone = new StaffPhone(dto.Phone);
            var sequencialNumber = await _repo.getLastSequencialNumber();
            var licenseNumber = StaffLicenseNumber.CreateFromPlainText(dto.Role[0], sequencialNumber + 1);
            var domain = _configuration["EmailSettings:BackofficeEmailDomain"];
            var staffEmail = StaffEmail.EmailBuilder(licenseNumber.Number, domain);
            StaffSpecialization specialization = (StaffSpecialization)Enum.Parse(typeof(StaffSpecialization), dto.Specialization);

            var staff = new Staff(dto.FirstName, dto.LastName, specialization, staffPhone, staffEmail, licenseNumber, sequencialNumber + 1);

            foreach (var timeSlotDto in dto.AvailabilitySlots)
            {
                var timeSlot = new TimeSlot(timeSlotDto.Start, timeSlotDto.End);  // Cria o VO TimeSlot
                staff.AddAvailabilitySlot(timeSlot);  // Adiciona o TimeSlot ao Staff
            }

            await this._repo.AddAsync(staff);

            await this._unitOfWork.CommitAsync();

            // Mapeia os TimeSlots para TimeSlotDto para retornar no StaffDto após a confirmação do commit
            var availabilitySlotsDto = staff.AvailabilitySlots
                .Select(slot => new TimeSlotDto { Start = slot.Starting, End = slot.Ending })
                .ToList();

            // Retorna os dados do Staff recém-criado
            return new StaffDto
            {
                Id = staff.Id.AsGuid(),  // Id gerado automaticamente
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                LicenseNumber = staff.LicenseNumber.Number,
                FullName = staff.FullName,
                Specialization = staff.Specialization.ToString(),
                StaffEmail = staff.Email.Value,  // Email gerado automaticamente
                Phone = staff.Phone.Value,
                AvailabilitySlots = availabilitySlotsDto
            };
        }

        public async Task<StaffDto> UpdateAsync(StaffLicenseNumber licenseNumber, EditStaffDto dto)
        {
            // procura o Staff pelo LicenseNumber fornecido
            var staff = await _repo.GetByLicenseNumberAsync(licenseNumber);

            if (staff == null)
                return null;

            // Atualiza o telefone se for fornecido
            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                staff.ChangePhone(new StaffPhone(dto.Phone));  // Converte para StaffPhone (VO) e aplica a validação necessária
            }

            // Atualiza os availability slots se forem fornecidos
            if (dto.AvailabilitySlots != null && dto.AvailabilitySlots.Any())
            {
                staff.ClearAvailabilitySlots();  // Limpa os slots existentes antes de adicionar os novos

                foreach (var timeSlotDto in dto.AvailabilitySlots)
                {
                    var timeSlot = new TimeSlot(timeSlotDto.Start, timeSlotDto.End);  // Cria o VO TimeSlot
                    staff.AddAvailabilitySlot(timeSlot);
                }
            }



            await _unitOfWork.CommitAsync();

            // Retorna o StaffDto atualizado
            var availabilitySlotsDto = staff.AvailabilitySlots
                .Select(slot => new TimeSlotDto { Start = slot.Starting, End = slot.Ending })
                .ToList();

            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Specialization = staff.Specialization.ToString(),
                StaffEmail = staff.Email.Value,
                Phone = staff.Phone.Value,
                AvailabilitySlots = availabilitySlotsDto
            };
        }

        public async Task<StaffDto> DeleteStaffAsync(string licenseNumber)
        {
            // Cria a instância de StaffLicenseNumber a partir de uma string existente
            var staffLicenseNumber = StaffLicenseNumber.CreateFromExistingString(licenseNumber);

            // procurao Staff pelo LicenseNumber
            var staff = await _repo.GetByLicenseNumberAsync(staffLicenseNumber);

            if (staff == null)
            {
                return null; // Staff não encontrado
            }

            // Verifica se o Staff ainda está ativo, caso haja algum critério para ser removido
            if (!staff.IsActive)
            {
                throw new BusinessRuleValidationException("The staff profile is already inactive.");
            }

            // Realiza a remoção
            _repo.Remove(staff);


            await _unitOfWork.CommitAsync();

            // Retorna o DTO do Staff removido (para confirmação ou log)
            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                Specialization = staff.Specialization.ToString(),
                StaffEmail = staff.Email.Value,
                Phone = staff.Phone.Value,
                AvailabilitySlots = staff.AvailabilitySlots.Select(slot => new TimeSlotDto
                {
                    Start = slot.Starting,
                    End = slot.Ending
                }).ToList()
            };
        }


    }
}