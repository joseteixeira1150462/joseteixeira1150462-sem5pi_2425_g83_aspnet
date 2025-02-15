using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthCare.Domain.Staffs;
using HealthCare.Domain.Users;

namespace HealthCare.Infraestructure.Staffs
{
    internal class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.OwnsOne(s => s.Email, email =>
            {
                email.Property(e => e.Value).HasMaxLength(255).IsRequired();
                email.HasIndex(e => e.Value)
                    .IsUnique();
            });

            builder.OwnsOne(s => s.LicenseNumber, ln =>
            {
                ln.Property(n => n.Number).HasMaxLength(10).IsRequired();
                ln.HasIndex(n => n.Number)
                   .IsUnique();
            });

            builder.OwnsOne(s => s.Phone, phone =>
            {
                phone.Property(p => p.Value).HasMaxLength(9).IsRequired();
                phone.HasIndex(p => p.Value)
                    .IsUnique();
            });

            builder.Property(s => s.Specialization).HasConversion<string>();

            builder.Property(s => s.SequencialNumber).IsRequired();

            builder.Ignore(s => s.FullName);

            builder.OwnsMany(s => s.AvailabilitySlots, ts =>
            {
                ts.WithOwner().HasForeignKey("StaffId");
                ts.Property(t => t.Starting).IsRequired();
                ts.Property(t => t.Ending).IsRequired();
            });

            builder.Property(s => s.UserId)
                .HasConversion(
                    userId => userId.AsString(),
                    value => new UserId(value)
                );
        }
    }
}

