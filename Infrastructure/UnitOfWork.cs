using System;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HealthCareDbContext _context;

        public UnitOfWork(HealthCareDbContext context)
        {
            this._context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await this._context.SaveChangesAsync();
        }
    }
}