using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Patients
{
    public class PatientEmail : ValueObject
    {
        public string Address { get; }

        public PatientEmail(string address)
        {
            ValidateEmail(address);
            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }

        public void ValidateEmail(string address) {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Email cannot be empty");
            }

            Regex AddressRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

            if (!AddressRegex.IsMatch(address))
            {
                throw new ArgumentException("Invalid email format");
            }
        }
    }
}