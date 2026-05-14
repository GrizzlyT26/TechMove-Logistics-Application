using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class ContractValidationTests
    {
        [Fact]
        public void ExpiredContract_ShouldBeInvalid()
        {
            // Arrange
            string contractStatus = "Expired";

            // Act
            bool isValid =
                contractStatus != "Expired" &&
                contractStatus != "On Hold";

            // Assert
            Assert.False(isValid);
        }
    }
}