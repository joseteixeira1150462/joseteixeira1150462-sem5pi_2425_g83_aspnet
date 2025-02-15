using System;
using HealthCare.Domain.Shared;
using Newtonsoft.Json;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationTypeVersionId : EntityId
    {
        [JsonConstructor]
        public OperationTypeVersionId(Guid value) : base(value)
        {
        }

        public OperationTypeVersionId(String value) : base(value)
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