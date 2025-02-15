using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthCare.Domain.Patients;

namespace HealthCare.Infrastructure.Patients
{
    public class PatientEntityTypeConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            // Configure the primary key
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.FullName)
                .HasMaxLength(101)
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .IsRequired();

            builder.Property(p => p.Gender)
                .HasMaxLength(10)
                .IsRequired();

            // Configure unique constraint on Medical Record Number
            builder.OwnsOne(p => p.MedicalRecordNumber, mrn =>
            {
                mrn.Property(m => m.Number)
                    .HasColumnName("MedicalRecordNumber")
                    .HasMaxLength(50)
                    .IsRequired();

                mrn.HasIndex(m => m.Number)
                   .IsUnique();  // Enforce uniqueness
            });

            // Configure Contact Information (Email, Phone) with uniqueness
            builder.OwnsOne(p => p.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("EmailAddress")
                    .HasMaxLength(255)
                    .IsRequired();

                email.HasIndex(e => e.Address)
                     .IsUnique();  // Enforce uniqueness
            });

            builder.OwnsOne(p => p.Phone, phone =>
            {
                phone.Property(ph => ph.Value)
                    .HasColumnName("Phone")
                    .HasMaxLength(15)
                    .IsRequired();

                phone.HasIndex(ph => ph.Value)
                     .IsUnique();  // Enforce uniqueness
            });

            // Configure optional allergies/medical conditions
            builder.Property(p => p.HealthConditions)
                .HasMaxLength(1000)
                .IsRequired(false);  // Optional field

            // Configure emergency contact
            builder.OwnsOne(p => p.EmergencyPhone, emergencyPhone =>
            {
                emergencyPhone.Property(ep => ep.Value)
                    .HasColumnName("EmergencyPhone")
                    .HasMaxLength(15)
                    .IsRequired();
            });

            builder.Property(p => p.SequentialNumber).IsRequired();
            builder.Property(p => p.UpdatesExecuted).HasColumnName("UpdatesExecuted")
                .IsRequired()
                .HasDefaultValue(0); // Valor inicial padrão
        }
    }
}
