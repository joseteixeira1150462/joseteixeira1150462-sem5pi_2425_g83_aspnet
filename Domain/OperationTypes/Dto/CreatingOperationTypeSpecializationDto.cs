using System;
using System.Collections.Generic;

namespace HealthCare.Domain.OperationTypes
{
    public class CreatingOperationTypeSpecializationDto
    {
        public int Quantity { get; set; }
        public string Role { get; set; }
        public string Specialization { get; set; }

        public CreatingOperationTypeSpecializationDto(
            int quantity,
            string role,
            string specialization
        ) {
            Quantity = quantity;
            Role = role;
            Specialization = specialization;
        }
    }
}