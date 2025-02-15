using System;

namespace HealthCare.Domain.Patients
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName {get;set;}

        public string LastName{get;set;}

        public string FullName {get;set;}

        public DateTime BirthDate {get;set;}

        public string Gender {get;set;}

        public string MedicalRecordNumber{get;set;}

        public string Email {get;set;}

        public string Phone {get;set;}

        public string HealthConditions {get;set;}

        public string EmergencyPhone {get;set;}

        public int UpdatesExecuted {get;set;}

        public int SequentialNumber {get; set;}
    }
}