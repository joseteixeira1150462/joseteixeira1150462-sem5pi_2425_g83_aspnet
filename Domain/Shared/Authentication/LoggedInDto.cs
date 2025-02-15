namespace HealthCare.Domain.Shared.Authentication
{
    public class LoggedInDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }

        public LoggedInDto (string token, string userId)
        {
            Token = token;
            UserId = userId;
        }
    }
}