using System;
using System.Collections.Generic;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.OperationTypes
{
   public class OperationTypeVersion : Entity<OperationTypeVersionId>
    {
        public DateTime? Starting { get; private set; } = null;
        public DateTime? Ending { get; private set; } = null;
        public OperationTypeDuration Duration { get; private set; }
        public List<OperationTypeSpecialization> Specializations { get; private set; } = new List<OperationTypeSpecialization>();

        public OperationTypeVersion() {}

        public OperationTypeVersion(
            int preparation,
            int operation,
            int cleaning
        ) {
            Id = new OperationTypeVersionId(Guid.NewGuid());

            Duration = new OperationTypeDuration(
                preparation,
                operation,
                cleaning
            );
        }

        public void AddSpecialization(OperationTypeSpecialization newSpecialization)
        {
            Specializations.Add(newSpecialization);
        }

        public void EndIt() {
            if (Ending != null)
                throw new BusinessRuleValidationException("This version is already deprecated");

            Ending = DateTime.Now;
        }

        public void StartIt() {
            if (Starting != null)
                throw new BusinessRuleValidationException("This version is already started");

            Starting = DateTime.Now; 
        }
    }
}