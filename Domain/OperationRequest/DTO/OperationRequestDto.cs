using System;
using System.Collections.Generic;

namespace HealthCare.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string OperationTypeId { get; set; }
        public string Priority { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}