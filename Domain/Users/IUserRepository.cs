using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Users
{
    public interface IUserRepository: IRepository<User, UserId>
    {
        public Task<List<User>> Find(Expression<Func<User, bool>> condition);
        public Task<User> GetByEmail(string email);
    }
}