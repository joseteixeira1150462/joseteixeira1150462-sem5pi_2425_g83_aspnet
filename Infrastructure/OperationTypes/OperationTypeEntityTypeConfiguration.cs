using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthCare.Domain.OperationTypes;

namespace HealthCare.Infraestructure.Staffs
{
    internal class OperationTypeEntityTypeConfiguration : IEntityTypeConfiguration<OperationType>
    {
        public void Configure(EntityTypeBuilder<OperationType> builder)
        {
            builder.HasKey(ot => ot.Id);

            builder.Property(ot => ot.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ot => ot.Active);

            builder.OwnsMany(ot => ot.Versions, v =>
            {
                v.HasKey(v => v.Id);
                v.WithOwner().HasForeignKey("OperationTypeId");
                
                v.Property(v => v.Starting);
                v.Property(v => v.Ending);

                v.OwnsOne(v => v.Duration, du =>
                {
                    du.Property(d => d.Preparation);
                    du.Property(d => d.Operation);
                    du.Property(d => d.Cleaning);
                });
                
                v.OwnsMany(v => v.Specializations, specs =>
                {
                    specs.WithOwner().HasForeignKey("OperationTypeVersionId");
                    specs.Property(spec => spec.Quantity);
                    specs.Property(spec => spec.Role);
                    specs.Property(spec => spec.Specialization);
                });
            });
        }
    }
}

