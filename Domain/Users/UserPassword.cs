using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HealthCare.Domain.Shared;

namespace HealthCare.Domain.Users
{
    public class UserPassword : ValueObject
    {
        public string Hash { get; private set; }

        private UserPassword(string hash)
        {
            Hash = hash;
        }

        public static UserPassword CreateFromPlainText(string password)
        {
            ValidatePassword(password);
            return new UserPassword(GetHashString(password));
        }

        public static UserPassword CreateFromHash(string hash)
        {
            return new UserPassword(hash);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Hash;
        }

        private static void ValidatePassword(string password) {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty");
            }

            Regex PasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s])[^ \n\r\t\f]{10,}$", RegexOptions.Compiled);

            if (!PasswordRegex.IsMatch(password)) {
                throw new BusinessRuleValidationException("Password must contain 1 capital letter, 1 digit and 1 special character");
            }
        }

        private static byte[] GetHash(string inputString) {
            using HashAlgorithm algorithm = SHA256.Create();

            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in GetHash(inputString)) {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}