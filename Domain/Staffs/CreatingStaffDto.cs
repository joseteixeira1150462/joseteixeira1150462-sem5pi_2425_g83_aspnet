using System.Collections.Generic;
using HealthCare.Domain.Shared.TimeSlot;

namespace HealthCare.Domain.Staffs
{
    public class CreatingStaffDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
        public List<TimeSlotDto> AvailabilitySlots { get; set; }
        public string Role { get; set; }

        public CreatingStaffDto(
            string firstName,
            string lastName,
            string specialization,
            string phone,
            List<TimeSlotDto> availabilitySlots,
            string role
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            Phone = phone;
            AvailabilitySlots = availabilitySlots;
            Role = role;
        }
    }
}