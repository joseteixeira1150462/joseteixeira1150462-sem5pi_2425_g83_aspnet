using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Staffs
{
    public class StaffPhone : ValueObject
    {
        public string Value { get; private set; }

        public StaffPhone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Phone cannot be empty");
            }


            if (!Regex.IsMatch(value, @"^\d{9}$")) // Exemplo para telefones de 10 dígitos
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