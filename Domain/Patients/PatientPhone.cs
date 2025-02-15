using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Patients
{
    public class PatientPhone : ValueObject
    {
        public string Value { get; private set; }

        public PatientPhone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Phone cannot be empty");
            }


            if (!Regex.IsMatch(value, @"^\+(\d{2,4})[\s]?(\d{9})$")) 
            {
                throw new ArgumentException("Invalid phone number format");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}