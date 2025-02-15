using System.Collections.Generic;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Staffs
{
    public class StaffEmail : ValueObject
    {
        public string Value { get; private set; }

        private StaffEmail(string value)
        {
            Value = value;
        }

        public static StaffEmail EmailBuilder(string licenseNumber, string domain)
        {
            return new StaffEmail(licenseNumber + "@" + domain);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}