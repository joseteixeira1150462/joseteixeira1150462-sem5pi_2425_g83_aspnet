using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Domain.Shared.TimeSlot;

namespace HealthCare.Domain.Staffs
{
    public class EditStaffDto
    {
        public string Phone { get; set; }  // Telefone do Staff como string
        public List<TimeSlotDto> AvailabilitySlots { get; set; }  // Lista de TimeSlots a serem atualizados

        public EditStaffDto()
        {
            AvailabilitySlots = new List<TimeSlotDto>();
        }
    }
}