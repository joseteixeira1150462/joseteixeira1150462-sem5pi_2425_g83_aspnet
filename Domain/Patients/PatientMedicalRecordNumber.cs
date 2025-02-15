using System;
using System.Collections.Generic;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Staffs
{
    public class PatientMedicalRecordNumber : ValueObject
    {
        public string Number { get; private set; }

        private PatientMedicalRecordNumber(string number)
        {
            Number = number;
        }

        public static PatientMedicalRecordNumber CreateFromPlainText(int month, int number)
        {
            string sequentialNumber = number.ToString().PadLeft(6, '0');
            string recordYear = DateTime.Now.Year.ToString();
            string recordMonth = DateTime.Now.AddMonths(-1).Month.ToString().PadLeft(2,'0'); //ensures the month is two digits formatted

            return new PatientMedicalRecordNumber(
               recordYear + recordMonth + sequentialNumber
            );
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

    }
}