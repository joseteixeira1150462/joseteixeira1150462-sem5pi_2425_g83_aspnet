using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HealthCare.Domain.Users;
using HealthCare.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User, UserId>, IUserRepository
    {

        public UserRepository(HealthCareDbContext context):base(context.Users)
        {
        }

        public async Task<List<User>> Find(Expression<Func<User, bool>> condition)
        {
            return await _objs.Where(condition).ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _objs.Include(u => u.StaffProfile).FirstOrDefaultAsync(u => u.Email.Address == email);
        }   
    }
}