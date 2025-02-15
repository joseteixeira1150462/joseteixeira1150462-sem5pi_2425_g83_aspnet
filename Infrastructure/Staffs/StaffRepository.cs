using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Domain.Staffs;
using HealthCare.Infrastructure;
using HealthCare.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Infraestructure.Staffs
{
    public class StaffRepository : BaseRepository<Staff, StaffId>, IStaffRepository
    {
        public StaffRepository(HealthCareDbContext context) : base(context.Staffs)
        {
        }

        public async Task<Staff> GetByIdEagerAsync(StaffId id)
        {
            return await _objs
                .Include(s => s.AvailabilitySlots).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Staff> GetByEmailAsync(string email)
        {
            return await _objs.Where(s => s.Email.Value == email).Include(s => s.User).FirstOrDefaultAsync();
        }

        public async Task<int> getLastSequencialNumber()
        {
            if (await _objs.AnyAsync())
            {
                return await _objs.MaxAsync(s => s.SequencialNumber);
            }

            return 0;
        }

        public async Task<Staff> GetByLicenseNumberAsync(StaffLicenseNumber licenseNumber)
        {
            return await _objs
                .Include(s => s.AvailabilitySlots)
                .FirstOrDefaultAsync(s => s.LicenseNumber.Number == licenseNumber.Number);
        }

        public async Task<List<Staff>> GetAllStaff()
        {
            return await _objs
                .Include(s => s.AvailabilitySlots)
                .ToListAsync();
        }

    }
}