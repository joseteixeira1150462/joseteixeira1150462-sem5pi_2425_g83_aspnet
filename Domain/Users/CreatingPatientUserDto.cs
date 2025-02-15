namespace HealthCare.Domain.Users
{
    public class CreatingPatientUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        
        public CreatingPatientUserDto(string email, string password ,string role) {
            Email = email;
            Password = password;
            Role = role;
        }
    }
}