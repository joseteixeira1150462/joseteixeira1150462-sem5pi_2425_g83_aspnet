using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Users;

namespace HealthCare.Domain.Patients
{
    public interface IPatientRepository : IRepository<Patient, PatientId>
    {
        public Task<int> getLastSequencialNumber();

       // public Task<int> GetLastUpdateExecutedNumberAsync();
        public Task<Patient> GetByIdEagerAsync(PatientId id);

        public Task<Patient> GetByEmailAsync(string email);

         public Task<Guid> GetPatientIdByUserIdAsync(UserId userId);

         public Task<List<ListPatientDto>> GetAllPatients();
    }
}