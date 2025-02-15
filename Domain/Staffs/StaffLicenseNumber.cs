using System;
using System.Collections.Generic;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Staffs
{
    public class StaffLicenseNumber : ValueObject
    {
        public string Number { get; private set; }

        private StaffLicenseNumber(string number)
        {
            Number = number;
        }

        public static StaffLicenseNumber CreateFromPlainText(char role, int number)
        {
            string numberToString = number.ToString().PadLeft(5, '0');
            string anoRecrutamento = DateTime.Now.Year.ToString();

            return new StaffLicenseNumber(
                role + anoRecrutamento + numberToString
            );
        }

        // Método de fábrica para criar uma instância a partir de uma string existente
        public static StaffLicenseNumber CreateFromExistingString(string existingNumber)
        {
            if (string.IsNullOrEmpty(existingNumber))
            {
                throw new ArgumentException("License number cannot be null or empty", nameof(existingNumber));
            }

            return new StaffLicenseNumber(existingNumber);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

    }
}