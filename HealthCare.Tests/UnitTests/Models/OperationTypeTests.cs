using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.Shared;

namespace HealthCare.Tests.UnitTests.Models
{
    public class OperationTypeTests
    {
        [Theory]
        [InlineData("ACL Reconstruction Surgery")]
        public void WhenPassingValidName_ThenIsInstantiated(string name)
        {
            new OperationType(name);
        }

        [Theory]
        [InlineData("")]
        public void WhenPassingEmptyName_ThenThrowsException(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new OperationType(name)
            );
            Assert.Equal("Operation Type name cannot be emtpy", ex.Message);
        }

        [Theory]
        [InlineData("ACL Reconstruction Surgery", 2, "Doctor", "Circulating", 30, 60, 30)]
        public void WhenAddingVersion_ThenLastVersionEndsAndNewStarts(
            string name,
            int quantity,
            string role,
            string specialization,
            int preparation,
            int operation,
            int cleaning
        ) {
            OperationTypeSpecialization spec = new(quantity, role, specialization);

            OperationTypeVersion version1 = new(preparation, operation, cleaning);
            version1.AddSpecialization(spec);

            OperationTypeVersion version2 = new(preparation, operation, cleaning);
            version2.AddSpecialization(spec);

            OperationType ot = new(name);

            ot.AddVersion(version1);
            ot.AddVersion(version2);

            Assert.NotNull(ot.Versions.First().Ending);
            Assert.NotNull(ot.Versions.Last().Starting);
        }

        [Theory]
        [InlineData("ACL Reconstruction Surgery")]
        public void MarkingAsActiveWhenActive_ThenThrowsException(string name)
        {
            OperationType ot = new(name);
            ot.MarkAsInactive();

            var ex = Assert.Throws<BusinessRuleValidationException>(() =>
                ot.MarkAsInactive()
            );
            Assert.Equal("Operation Type is already inactive", ex.Message);
        }
    }
}