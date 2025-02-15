using Microsoft.EntityFrameworkCore;
using HealthCare.Domain.Staffs;
using HealthCare.Infraestructure.Staffs;
using HealthCare.Domain.Users;
using HealthCare.Infrastructure.Users;
using HealthCare.Domain.Patients;
using HealthCare.Infrastructure.Patients;
using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.OperationRequests;
using HealthCare.Infrastructure.OperationRequests;

namespace HealthCare.Infrastructure
{
    public class HealthCareDbContext : DbContext
    {
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<OperationRequest> OperationRequest { get; set; }

        public HealthCareDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationRequestEntityTypeConfiguration());
        }
    }
}