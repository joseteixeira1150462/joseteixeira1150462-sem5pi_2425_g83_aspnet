using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthCare.Domain.OperationRequests;

namespace HealthCare.Infrastructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration : IEntityTypeConfiguration<OperationRequest>
    {
        public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            builder.HasKey(or => or.Id);

            builder.Property(or => or.PatientId)
                .IsRequired();

            builder.Property(or => or.DoctorId)
                .IsRequired();

            builder.Property(or => or.OperationTypeId)
                .IsRequired();

            builder.Property(or => or.Priority)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(or => or.DeadlineDate)
                .IsRequired();
        }
    }
}
