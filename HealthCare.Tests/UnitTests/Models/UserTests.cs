using HealthCare.Domain.Shared;
using HealthCare.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace HealthCare.Tests.UnitTests.Models
{
    public class UserTests
    {
        [Theory]
        [InlineData("tiago.sdg7@gmail.com", "Admin", "E'ea.R9uIFqw6]i")]
        [InlineData("John+John22@example.com", "Doctor", "EtTQb%3mB9c3.6$")]
        [InlineData("_12135.tiago@example.com", "Nurse", "3J}ik8Pc)epC_g,")]
        [InlineData("%2135@example.com", "Technician", "m-Kf;3nme(A+fDr")]
        [InlineData("1222135@example.com", "Patient", "6b&v-cI;a&vV7XM")]
        public void WhenPassingCorrectData_ThenIsInstantiated(string email, string role, string password)
        {
            User user = new(email, role);

            user.SetPassword(password);
            user.MarkAsActive();
        }

        [Theory]
        [InlineData("tia(go.sdg7@gmail.com", "Admin")]
        [InlineData("John+John22@.com", "Doctor")]
        [InlineData("_12135.tiago@example.c", "Nurse")]
        [InlineData("%2135@examplecom", "Technician")]
        [InlineData("@example.com", "Patient")]
        [InlineData("", "Patient")]
        public void WhenPassingInvalidEmail_ThenThrowsException(string email, string role)
        {
            if ( email.IsNullOrEmpty() )
            {
                var ex = Assert.Throws<ArgumentException>(() =>
                    new User(email, role)
                );
                Assert.Equal("Email cannot be empty", ex.Message);
            }
            else
            {
                var ex = Assert.Throws<BusinessRuleValidationException>(() =>
                    new User(email, role)
                );
                Assert.Equal("Invalid email format", ex.Message);
            }
        }

        [Theory]
        [InlineData("tiago.sdg7@gmail.com", "Admin", "E'ea.R")]
        [InlineData("John+John22@example.com", "Doctor", "EtTQb%mBc.$")]
        [InlineData("_12135.tiago@example.com", "Nurse", "3Jik8PcepCg")]
        [InlineData("%2135@example.com", "Technician", "m-f;3nme(+fr")]
        [InlineData("%2135@example.com", "Technician", "")]
        public void WhenPassingInvalidPassword_ThenThrowsException(string email, string role, string password)
        {
            User user = new(email, role);

            if ( password.IsNullOrEmpty() )
            {
                var ex = Assert.Throws<ArgumentException>(() =>
                    user.SetPassword(password)
                );
                Assert.Equal("Password cannot be empty", ex.Message);
            }
            else
            {
                var ex = Assert.Throws<BusinessRuleValidationException>(() =>
                    user.SetPassword(password)
                );
                Assert.Equal("Password must contain 1 capital letter, 1 digit and 1 special character", ex.Message);
            }
        }

        [Theory]
        [InlineData("tiago.sdg7@gmail.com", "Admin")]
        public void MarkingAsActiveWhenActive_ThenThrowsException(string email, string role)
        {
            User user = new(email, role);
            user.MarkAsActive();

            var ex = Assert.Throws<BusinessRuleValidationException>(() =>
                user.MarkAsActive()
            );
            Assert.Equal("User has already been activated", ex.Message);
        }
    }
}