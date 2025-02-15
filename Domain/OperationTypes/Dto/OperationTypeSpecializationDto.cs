using System;
using System.Collections.Generic;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeSpecializationDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Role { get; set; }
        public string Specialization { get; set; }

        public OperationTypeSpecializationDto(
            Guid id,
            int quantity,
            string role,
            string specialization
        ) {
            Id = id;
            Quantity = quantity;
            Role = role;
            Specialization = specialization;
        }
    }
}