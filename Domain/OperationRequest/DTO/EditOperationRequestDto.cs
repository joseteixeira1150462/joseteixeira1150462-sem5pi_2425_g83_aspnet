using System;
using System.Collections.Generic;
using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.Patients;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.OperationRequests
{
    public class EditOperationRequestDto
    {
        public Guid Id { get; set; }
        public PatientId PatientId { get; set; }
        public StaffId DoctorId { get; set; }
        public OperationTypeId OperationTypeId { get; set; }
        public OperationRequestPriority Priority { get; set; }
        public DateTime DeadlineDate { get; set; }
        
        public EditOperationRequestDto(
            Guid id,
            PatientId patientId,
            StaffId doctorId,
            OperationTypeId operationTypeId,
            OperationRequestPriority priority,
            DateTime deadLineDate
            
        ) {
            Id = id;
            PatientId = patientId;
            DoctorId = doctorId;
            OperationTypeId = operationTypeId;
            Priority = priority;
            DeadlineDate = deadLineDate;
        }
    }
}