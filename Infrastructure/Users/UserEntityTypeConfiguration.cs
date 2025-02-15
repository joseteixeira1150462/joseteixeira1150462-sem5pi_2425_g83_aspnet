using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthCare.Domain.Users;
using HealthCare.Domain.Staffs;
using HealthCare.Domain.Patients;

namespace HealthCare.Infrastructure.Users
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Role).HasConversion<string>().IsRequired();
            builder.Property(u => u.LoginAttempts).IsRequired();
            builder.Property(u => u.LastRequest).IsRequired();
            builder.Property(u => u.Active).IsRequired();

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address).HasMaxLength(100).IsRequired();
            });

            builder.OwnsOne(u => u.PasswordHash, ph =>
            {
                ph.Property(h => h.Hash).HasMaxLength(256).IsRequired();
            });

            builder.HasOne(u => u.StaffProfile)
                .WithOne(s => s.User)
                .HasForeignKey<Staff>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u=> u.PatientProfile)
                .WithOne (p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);    
        }
    }
}