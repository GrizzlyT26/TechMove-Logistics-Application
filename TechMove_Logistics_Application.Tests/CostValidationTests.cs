using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class CostValidationTests
    {
        [Fact]
        public void Cost_ShouldBePositive()
        {
            decimal amount = 2500m;

            bool valid = amount > 0;

            Assert.True(valid);
        }

        [Fact]
        public void Cost_ShouldRejectNegativeAmounts()
        {
            decimal amount = -500m;

            bool valid = amount > 0;

            Assert.False(valid);
        }

        [Fact]
        public void Cost_ShouldRejectZeroAmount()
        {
            decimal amount = 0m;

            bool valid = amount > 0;

            Assert.False(valid);
        }
    }
}