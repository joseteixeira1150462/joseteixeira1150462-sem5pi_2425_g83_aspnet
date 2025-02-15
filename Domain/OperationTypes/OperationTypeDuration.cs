using System.Collections.Generic;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeDuration : ValueObject
    {
        public int Preparation { get; set; }
        public int Operation { get; set; }
        public int Cleaning { get; set; }

        public OperationTypeDuration(
            int preparation,
            int operation,
            int cleaning
        ) {
            if ( preparation <= 0 || operation <= 0 || cleaning <= 0 ) 
                throw new ArgumentException("Duration must be > 0");

            Preparation = preparation;
            Operation = operation;
            Cleaning = cleaning;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Preparation;
            yield return Operation;
            yield return Cleaning;
        }
    }
}