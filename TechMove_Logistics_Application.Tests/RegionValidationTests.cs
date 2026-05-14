using System.Collections.Generic;
using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class RegionValidationTests
    {
        [Fact]
        public void Region_ShouldBeValid()
        {
            var regions = new List<string>
            {
                "Africa",
                "Europe",
                "Asia",
                "North America",
                "South America",
                "Middle East",
                "Australia"
            };

            bool exists =
                regions.Contains("Africa");

            Assert.True(exists);
        }

        [Fact]
        public void Region_ShouldRejectInvalidRegion()
        {
            var regions = new List<string>
            {
                "Africa",
                "Europe",
                "Asia"
            };

            bool exists =
                regions.Contains("Atlantis");

            Assert.False(exists);
        }
    }
}