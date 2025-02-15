using System.Collections.Generic;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeSpecialization : Entity<OperationTypeSpecializationId>
    {
        public int Quantity { get; set; }
        public UserRole Role { get; set; }
        public StaffSpecialization Specialization { get; set; }

        public OperationTypeSpecialization() {}
        
        public OperationTypeSpecialization(
            int quantity,
            string role,
            string specialization
        ) {
            Id = new OperationTypeSpecializationId(Guid.NewGuid());

            if ( quantity <= 0 )
                throw new ArgumentException("Quantity has to be greater than 0");

            Quantity = quantity;
            Role = (UserRole)Enum.Parse(typeof(UserRole), role);
            Specialization = (StaffSpecialization)Enum.Parse(typeof(StaffSpecialization), specialization);
        }
    }
}