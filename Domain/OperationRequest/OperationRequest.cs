using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.Patients;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.OperationRequests
{
    public class OperationRequest : Entity<OperationRequestId>, IAggregateRoot
    {
        public PatientId PatientId { get; private set; }
        public StaffId DoctorId { get; private set; }
        public OperationTypeId OperationTypeId { get; private set; }
        public OperationRequestPriority Priority { get; private set; }
        public DateTime DeadlineDate { get; private set; }

        public OperationRequest(
                                PatientId patientId,
                                StaffId doctorId,
                                OperationTypeId operationTypeId,
                                OperationRequestPriority priority,
                                DateTime deadlineDate
                                )
        {
            Id = new OperationRequestId(Guid.NewGuid());

            PatientId = patientId;
            DoctorId = doctorId;
            OperationTypeId = operationTypeId;
            Priority = priority;
            DeadlineDate = deadlineDate;
        }

        public void ChangePatientId(PatientId newPatientId)
        {
            if (newPatientId == null)
            {
                throw new ArgumentNullException(nameof(newPatientId), "PatientId cannot be null");
            }

            PatientId = newPatientId;
        }

        public void ChangeDoctorId(StaffId newDoctorId)
        {
            if (newDoctorId == null)
            {
                throw new ArgumentNullException(nameof(newDoctorId), "DoctorId cannot be null");
            }

            DoctorId = newDoctorId;
        }

        public void ChangeOperationTypeId(OperationTypeId newOperationTypeId)
        {
            if (newOperationTypeId == null)
            {
                throw new ArgumentNullException(nameof(newOperationTypeId), "OperationTypeId cannot be null");
            }

            OperationTypeId = newOperationTypeId;
        }

        public void ChangePriority(OperationRequestPriority newPriority)
        {
            if (newPriority == null)
            {
                throw new ArgumentNullException(nameof(newPriority), "Priority cannot be null");
            }

            Priority = newPriority;
        }

        public void ChangeDeadlineDate(DateTime newDeadlineDate)
        {
            if (newDeadlineDate == DateTime.MinValue)
            {
                throw new ArgumentException("DeadlineDate cannot be the default value", nameof(newDeadlineDate));
            }

            DeadlineDate = newDeadlineDate;
        }
    }
}