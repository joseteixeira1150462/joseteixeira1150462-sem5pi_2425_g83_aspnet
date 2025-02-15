using System;

namespace HealthCare.Domain.Users
{
    public class UpdateUserPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UpdateUserPasswordDto(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}