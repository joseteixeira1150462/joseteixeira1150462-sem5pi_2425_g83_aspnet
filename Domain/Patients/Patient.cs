using System;
using System.Linq;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;
using HealthCare.Domain.Users;

namespace HealthCare.Domain.Patients
{
    public class Patient : Entity<PatientId>, IAggregateRoot
    {
        public string FirstName {get;set;}

        public string LastName{get;set;}

        public string FullName {get;set;}

        public DateTime BirthDate {get;set;}

        public string Gender {get;set;}

        public PatientMedicalRecordNumber MedicalRecordNumber{get;set;}

        public PatientEmail Email {get;set;}

        public PatientPhone Phone {get;set;}

        public string HealthConditions {get;set;}

        public PatientPhone EmergencyPhone {get;set;}

        public int SequentialNumber {get;set;}

        public int UpdatesExecuted {get;set;}

        public UserId UserId { get; private set; }
        public virtual User User { get; private set; }

        public Patient(){}

        public Patient(
            string firstName,
            string lastName,
            DateTime birthDate,
            string gender,
            PatientMedicalRecordNumber medicalRecordNumber,
            PatientEmail email,
            PatientPhone phone,
            PatientPhone emergencyPhone,
            int sequentialNumber,
            int updatesExecuted
        ){
            Id = new PatientId(Guid.NewGuid()); 
            
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{FirstName} {LastName}";

            if (!IsValidGender(gender))
                throw new ArgumentException("Invalid gender", nameof(gender));

            BirthDate = birthDate;
            Gender = gender;
            MedicalRecordNumber = medicalRecordNumber;
            Email = email;
            Phone = phone;
            EmergencyPhone = emergencyPhone;

            SequentialNumber = sequentialNumber;
            UpdatesExecuted = updatesExecuted; //by default updates are not executed
        }

        
        private bool IsValidGender(string gender)
        {
            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase);
        }

          public void AssociateUser(User user) {
            if (User != null)
            {
                throw new BusinessRuleValidationException("The Patient profile with e-mail: " + user.Email.Address + " already has a User");
            }

            if (user.Email.Address != Email.Address) {
                throw new BusinessRuleValidationException("There isn\'t a Patient profile with e-mail address: " + user.Email.Address);
            }

            UserId = user.Id;
            User = user;
        }

        public void IncrementUpdateAttempts(){
            this.UpdatesExecuted++;
        }
    }

}