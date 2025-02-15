using HealthCare.Domain.OperationTypes;

namespace HealthCare.Tests.UnitTests.Models
{
    public class OperationTypeSpecializationTests
    {
        [Theory]
        [InlineData(2, "Doctor", "Circulating")]
        public void WhenPassingCorrectData_ThenIsInstantiated(int quantity, string role, string specialization)
        {
            new OperationTypeSpecialization(quantity, role, specialization);
        }

        [Theory]
        [InlineData(0, "Doctor", "Circulating")]
        [InlineData(-2, "Doctor", "Circulating")]
        public void WhenPassingNegativeQuantity_ThenThrowsException(int quantity, string role, string specialization)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new OperationTypeSpecialization(quantity, role, specialization)
            );
            Assert.Equal("Quantity has to be greater than 0", ex.Message);
        }
    }
}