using System;
using HealthCare.Domain.Shared;
using Newtonsoft.Json;

namespace HealthCare.Domain.Staffs
{
    public class TimeSlotId : EntityId
    {
        [JsonConstructor]
        public TimeSlotId(Guid value) : base(value)
        {
        }

        public TimeSlotId(String value) : base(value)
        {
        }

        override
        protected  Object createFromString(String text){
            return new Guid(text);
        }

        override
        public String AsString(){
            Guid obj = (Guid) base.ObjValue;
            return obj.ToString();
        }
        
       
        public Guid AsGuid(){
            return (Guid) base.ObjValue;
        }
    }
}