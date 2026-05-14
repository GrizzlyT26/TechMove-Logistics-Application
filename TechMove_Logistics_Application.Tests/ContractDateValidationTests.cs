using System;
using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class ContractDateValidationTests
    {
        [Fact]
        public void ContractDates_ShouldBeValid()
        {
            DateTime start =
                new DateTime(2026, 5, 1);

            DateTime end =
                new DateTime(2026, 5, 10);

            bool valid = end > start;

            Assert.True(valid);
        }

        [Fact]
        public void ContractDates_ShouldRejectInvalidDates()
        {
            DateTime start =
                new DateTime(2026, 5, 10);

            DateTime end =
                new DateTime(2026, 5, 1);

            bool valid = end > start;

            Assert.False(valid);
        }

        [Fact]
        public void ContractDates_ShouldRejectSameDates()
        {
            DateTime start =
                new DateTime(2026, 5, 10);

            DateTime end =
                new DateTime(2026, 5, 10);

            bool valid = end > start;

            Assert.False(valid);
        }
    }
}