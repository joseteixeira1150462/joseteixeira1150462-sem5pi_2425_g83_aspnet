

using Microsoft.EntityFrameworkCore.Internal;

namespace HealthCare.Domain.Patients{

    public class EditingPatientDto{

        public EditingPatientDto(){

        }

        public EditingPatientDto(string firstName, string lastName, DateTime birthDate, string gender, 
         string email, string phone, string emergencyPhone)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Email = email;
            Phone = phone;
            EmergencyPhone = emergencyPhone;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string EmergencyPhone { get; set; }
    }
}