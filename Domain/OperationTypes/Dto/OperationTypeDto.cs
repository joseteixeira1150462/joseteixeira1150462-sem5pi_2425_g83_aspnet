using System;
using System.Collections.Generic;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Preparation { get; set; }
        public int Operation { get; set; }
        public int Cleaning { get; set; }
        public List<OperationTypeSpecializationDto> Specializations { get; set; }

        public OperationTypeDto(
            Guid id,
            string name,
            bool status,
            int preparation,
            int operation,
            int cleaning,
            List<OperationTypeSpecializationDto> specializations
        ) {
            Id = id;
            Name = name;
            Active = status;
            Preparation = preparation;
            Operation = operation;
            Cleaning = cleaning;
            Specializations = specializations;
        }
    }
}