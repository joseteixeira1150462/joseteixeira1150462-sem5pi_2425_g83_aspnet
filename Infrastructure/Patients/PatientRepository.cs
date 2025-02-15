using System;
using System.Threading.Tasks;
using HealthCare.Domain.Patients;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Users;
using HealthCare.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HealthCare.Infrastructure.Patients
{
    public class PatientRepository : BaseRepository<Patient, PatientId>, IPatientRepository

    {


        public PatientRepository(HealthCareDbContext context) : base(context.Patients)
        {

        }

        public async Task<Patient> GetByEmailAsync(string email)
        {
            return await _objs
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Email.Address == email);
        }

        // Eagerly get a patient by id (includes related entities if necessary)
        public async Task<Patient> GetByIdEagerAsync(PatientId id)
        {
            return await _objs
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> GetPatientIdByUserIdAsync(UserId userId)
        {
            return await _objs
                .Where(p => p.UserId == userId)
                .Select(p => p.Id.AsGuid())
                .FirstOrDefaultAsync();
        }

        // Get the last sequential number for patients
        public async Task<int> getLastSequencialNumber()
        {
            if (await _objs.AnyAsync())
            {
                return await _objs.MaxAsync(p => p.SequentialNumber);
            }

            return 0;
        }

        public async Task<List<ListPatientDto>> GetAllPatients()
        {
            return await _objs
                .Select(p => new ListPatientDto
                {
                    Id = p.Id.AsGuid(),
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    FullName = $"{p.FirstName} {p.LastName}", // Or use p.FullName if available
                    BirthDate = p.BirthDate,
                    Gender = p.Gender,
                    MedicalRecordNumber = p.MedicalRecordNumber.Number,
                    Email = p.Email.Address,
                    Phone = p.Phone.Value,
                    EmergencyPhone = p.EmergencyPhone.Value, // Ensure this maps correctly
                    UpdatesExecuted = p.UpdatesExecuted,
                    SequentialNumber = p.SequentialNumber
                })
                .ToListAsync();
        }
    }
}
