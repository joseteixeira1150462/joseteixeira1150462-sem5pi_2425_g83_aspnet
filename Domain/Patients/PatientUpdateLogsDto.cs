namespace HealthCare.Domain.Patients
{
    public class PatientUpdateLogsDto
    {
        public PatientDto OldValue { get; set; }
        public PatientDto NewValue { get; set; }
        public string EditDate { get; set; }
        public string Description {get; set;}

        public PatientUpdateLogsDto(PatientDto oldValue, PatientDto newValue){
            OldValue = oldValue;
            NewValue = newValue;
            EditDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            Description = null;
        }
    }
}