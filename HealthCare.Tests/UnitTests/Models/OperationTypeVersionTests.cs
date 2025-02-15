using HealthCare.Domain.OperationTypes;

namespace HealthCare.Tests.UnitTests.Models
{
    public class OperationTypeVersionTests
    {
        [Theory]
        [InlineData(30, 60, 20)]
        public void WhenPassingCorrectData_ThenIsInstantiated(int preparation, int operation, int cleaning)
        {
            new OperationTypeVersion(preparation, operation, cleaning);
        }

        [Theory]
        [InlineData(-30, 60, 20)]
        [InlineData(-30, -60, 20)]
        [InlineData(-30, -60, -20)]
        [InlineData(-30, 60, -20)]
        [InlineData(30, 60, -20)]
        [InlineData(30, -60, -20)]
        public void WhenPassingNegativeData_ThenThrowsException(int preparation, int operation, int cleaning)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new OperationTypeVersion(preparation, operation, cleaning)
            );
            Assert.Equal("Duration must be > 0", ex.Message);
        }
    }
}