using HealthCare.Domain.Shared;
using Microsoft.IdentityModel.Tokens;
using Xunit.Sdk;

namespace HealthCare.Domain.OperationTypes
{
    public class OperationType : Entity<OperationTypeId>, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }
        public List<OperationTypeVersion> Versions { get; private set; } = new List<OperationTypeVersion>();

        public OperationType(string name)
        {
            Id = new OperationTypeId(Guid.NewGuid());

            if (name.IsNullOrEmpty())
                throw new ArgumentException("Operation Type name cannot be emtpy");
                
            Name = name;
            Active = true;
        }

        public void AddVersion(OperationTypeVersion newVersion)
        {
            if (Versions.Any())
                Versions.Last().EndIt();
            
            newVersion.StartIt();
            Versions.Add(newVersion);
        }

        public void MarkAsInactive()
        {
            if ( !Active )
                throw new BusinessRuleValidationException("Operation Type is already inactive");

            Active = false;
        }
    }
}