using System;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.Shared.TimeSlot
{
   public class TimeSlot : Entity<TimeSlotId>
    {
        public DateTime Starting { get; private set; }
        public DateTime Ending { get; private set; }

        public TimeSlot(DateTime starting, DateTime ending) {

            if (starting >= ending)
            {
                throw new ArgumentException("Starting time must be before the ending time.");
            }

            Id = new TimeSlotId(Guid.NewGuid());

            Starting = starting;
            Ending = ending;
        }

        public bool InTimeSlot(DateTime dateTime) {
            return dateTime.CompareTo(Starting) >= 0 && dateTime.CompareTo(Ending) <= 0;
        }

        // Verifica se dois TimeSlots entram em conflito (sobrepõem-se)
        public bool ConflictsWith(TimeSlot other)
        {
            return Starting < other.Ending && Ending > other.Starting;
        }
    }
}