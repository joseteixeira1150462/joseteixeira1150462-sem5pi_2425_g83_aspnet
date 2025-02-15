using System;
using HealthCare.Domain.Shared;
using Newtonsoft.Json;

namespace HealthCare.Domain.Users
{
    public class UserId : EntityId
    {
        [JsonConstructor]
        public UserId(Guid value) : base(value)
        {
        }

        public UserId(String value) : base(value)
        {
        }

        override
        protected  Object createFromString(String text){
            return new Guid(text);
        }

        override
        public string AsString(){
            Guid obj = (Guid) base.ObjValue;
            return obj.ToString();
        }
        
       
        public Guid AsGuid(){
            return (Guid) base.ObjValue;
        }
    }
}