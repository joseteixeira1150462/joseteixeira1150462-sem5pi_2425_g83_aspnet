using System;
using System.Collections.Generic;

namespace HealthCare.Domain.OperationTypes
{
    public class CreatingOperationTypeDto
    {
        public string Name { get; set; }
        public int Preparation { get; set; }
        public int Operation { get; set; }
        public int Cleaning { get; set; }
        public List<OperationTypeSpecializationDto> Specializations { get; set; }

        public CreatingOperationTypeDto(
            string name,
            int preparation,
            int operation,
            int cleaning,
            List<OperationTypeSpecializationDto> specializations
        ) {
            Name = name;
            Preparation = preparation;
            Operation = operation;
            Cleaning = cleaning;
            Specializations = specializations;
        }
    }
}