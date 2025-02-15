using HealthCare.Domain.Patients;
using Newtonsoft.Json;

namespace HealthCare.Logs
{
       public class LogChangesService
    {
        private readonly string _logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

        public LogChangesService()
        {
            // Criação de diretório para os logs se não existir
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }
        }

         public async Task AppendPatientLogAsync(PatientDto oldValue, PatientDto newValue, PatientId patientId, string description = null)
        {
            var logFilePath = Path.Combine(_logFolderPath, $"{patientId.AsGuid()}_patient_update_logs.json");

            var existingLogs = new List<PatientUpdateLogsDto>();

            // Se o arquivo de log já existir, leia os logs existentes
            if (File.Exists(logFilePath))
            {
                var currentLogs = await File.ReadAllTextAsync(logFilePath);
                existingLogs = JsonConvert.DeserializeObject<List<PatientUpdateLogsDto>>(currentLogs) ?? new List<PatientUpdateLogsDto>();
            }

            // Adiciona o novo log à lista
            var logEntry = new PatientUpdateLogsDto(oldValue, newValue)
            {
                Description = description
            };

            existingLogs.Add(logEntry);

            // Serializa os logs existentes com o novo log e escreve de volta no arquivo
            var updatedLogsJson = JsonConvert.SerializeObject(existingLogs, Formatting.Indented);
            await File.WriteAllTextAsync(logFilePath, updatedLogsJson);
        }

        public async Task AppendEmailUpdateLogAsync(PatientDto oldPatient, string newEmail, PatientId patientId)
        {
            // Cria uma cópia do paciente para o novo valor com o email atualizado
            var newPatient = new PatientDto
            {
                Id = oldPatient.Id,
                FirstName = oldPatient.FirstName,
                LastName = oldPatient.LastName,
                FullName = oldPatient.FullName,
                BirthDate = oldPatient.BirthDate,
                Gender = oldPatient.Gender,
                MedicalRecordNumber = oldPatient.MedicalRecordNumber,
                Email = newEmail,
                Phone = oldPatient.Phone,
                HealthConditions = oldPatient.HealthConditions,
                EmergencyPhone = oldPatient.EmergencyPhone,
                UpdatesExecuted = oldPatient.UpdatesExecuted + 1,
                SequentialNumber = oldPatient.SequentialNumber
            };

            // Monta a mensagem personalizada para o log
            var description = $"[User Verified] Email updated from {oldPatient.Email} to {newEmail} on {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}";

            // Chama o método genérico de log para registrar a alteração
            await AppendPatientLogAsync(oldPatient, newPatient, patientId, description);
        }

    }

}

    


