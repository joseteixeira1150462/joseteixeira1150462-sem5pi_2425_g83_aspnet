using System.Collections.Generic;
using HealthCare.Domain.Shared.TimeSlot;

namespace HealthCare.Domain.OperationRequests
{
    public class CreateOperationRequestDto
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string OperationTypeId { get; set; }
        public string OperationRequestPriority { get; set; }
        public DateTime DeadlineDate { get; set; }

        public CreateOperationRequestDto(
            string patientId,
            string doctorId,
            string operationTypeId,
            string operationRequestPriority,
            DateTime deadlineDate
        )
        {
            PatientId = patientId;
            DoctorId = doctorId;
            OperationTypeId = operationTypeId;
            OperationRequestPriority = operationRequestPriority;
            DeadlineDate = deadlineDate;
        }
    }
}