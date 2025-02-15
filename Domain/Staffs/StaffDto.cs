using System;
using System.Collections.Generic;
using HealthCare.Domain.Shared.TimeSlot;

namespace HealthCare.Domain.Staffs
{
    public class StaffDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string LicenseNumber { get; set; }
        public string Specialization { get; set; }
        public string StaffEmail { get; set; }
        public string Phone { get; set; }
        public List<TimeSlotDto> AvailabilitySlots { get; set; }
    }
}