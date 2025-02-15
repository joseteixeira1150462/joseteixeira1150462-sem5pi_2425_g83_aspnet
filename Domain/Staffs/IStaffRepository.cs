using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Staffs
{
    public interface IStaffRepository : IRepository<Staff, StaffId>
    {
        public Task<List<Staff>> GetAllStaff();
        public Task<int> getLastSequencialNumber();
        public Task<Staff> GetByEmailAsync(string email);
        public Task<Staff> GetByIdEagerAsync(StaffId id);
        public Task<Staff> GetByLicenseNumberAsync(StaffLicenseNumber licenseNumber);
    }
}