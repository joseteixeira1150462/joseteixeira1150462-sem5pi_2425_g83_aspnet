using System;
using Microsoft.AspNetCore.SignalR;

namespace HealthCare.Domain.Users
{
    public class InternalUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public string PasswordHash { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime LastRequest { get; set; }

        public InternalUserDto(
            Guid id,
            string email,
            string role,
            bool status,
            string hash,
            int loginAttempts,
            DateTime lastRequest
        ) {
            Id = id;
            Email = email;
            Role = role;
            Active = status;
            PasswordHash = hash;
            LoginAttempts = loginAttempts;
            LastRequest = lastRequest;
        }
    }
}