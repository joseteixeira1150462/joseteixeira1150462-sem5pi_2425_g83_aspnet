namespace HealthCare.Domain.Users
{
    public class CreatingUserDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        
        public CreatingUserDto(string email, string role) {
            Email = email;
            Role = role;
        }
    }
}