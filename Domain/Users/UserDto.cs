using System;
using Microsoft.AspNetCore.SignalR;

namespace HealthCare.Domain.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }

        public UserDto(Guid id, string email, string role, bool status) {
            Id = id;
            Email = email;
            Role = role;
            Active = status;
        }
    }
}