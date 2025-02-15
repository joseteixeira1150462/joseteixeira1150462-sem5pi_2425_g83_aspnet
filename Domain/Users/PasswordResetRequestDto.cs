namespace HealthCare.Domain.Users
{
    public class PasswordResetRequestDto
    {
        public string Email { get; set; }

        public PasswordResetRequestDto(string email) {
            Email = email;
        }
    }
}