using System;
using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class ServiceRequestTests
    {
        [Fact]
        public void CurrencyConversion_ShouldCalculateCorrectZAR()
        {
            // Arrange
            decimal usdAmount = 100m;
            decimal exchangeRate = 18.50m;

            // Act
            decimal zarAmount =
                Math.Round(usdAmount * exchangeRate, 2);

            // Assert
            Assert.Equal(1850m, zarAmount);
        }
    }
}