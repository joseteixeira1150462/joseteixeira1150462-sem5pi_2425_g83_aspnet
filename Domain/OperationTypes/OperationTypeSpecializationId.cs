using System;
using HealthCare.Domain.Shared;
using Newtonsoft.Json;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeSpecializationId : EntityId
    {
        [JsonConstructor]
        public OperationTypeSpecializationId(Guid value) : base(value)
        {
        }

        public OperationTypeSpecializationId(String value) : base(value)
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