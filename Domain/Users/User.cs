using HealthCare.Domain.Patients;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.Users
{
    public class User : Entity<UserId>, IAggregateRoot
    {
        public UserPassword PasswordHash { get; private set; }
        public UserEmail Email { get; set; }
        public UserRole Role { get; private set; }
        public bool Active { get; private set; }
        public virtual Staff StaffProfile { get; private set; }
        public int LoginAttempts { get; private set; }
        public DateTime LastRequest { get; private set; }
        public virtual Patient PatientProfile { get; private set; }

        private User() { }

        public User(string email, string roleInput)
        {
            Id = new UserId(Guid.NewGuid());

            Email = new UserEmail(email);
            Role = (UserRole)Enum.Parse(typeof(UserRole), roleInput);
            Active = false;
        }

        public void SetPassword(string password)
        {
            PasswordHash = UserPassword.CreateFromPlainText(password);
        }

        public void MarkAsActive()
        {
            if (Active)
            {
                throw new BusinessRuleValidationException("User has already been activated");
            }

            Active = true;
        }

        public void MarkAsInactive(){
            if(!Active)
            {
                throw new BusinessRuleValidationException("User has already been deactivated");
            }
            Active = false;
        }

        public void RefreshSession()
        {
            LastRequest = DateTime.Now;
        }

        public void ResetLoginAttempts()
        {
            LoginAttempts = 5;
        }
        public void DecrementLoginAttempts()
        {
            LoginAttempts--;
        }
    }
}