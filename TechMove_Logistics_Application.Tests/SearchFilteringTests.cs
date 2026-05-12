using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class SearchFilteringTests
    {
        [Fact]
        public void Search_ShouldReturnMatchingContracts()
        {
            var contracts = new List<string>
            {
                "Premium",
                "Basic",
                "Enterprise"
            };

            var result = contracts
                .Where(c => c.Contains("Premium"))
                .ToList();

            Assert.Single(result);
        }

        [Fact]
        public void Search_ShouldReturnNoMatches()
        {
            var contracts = new List<string>
            {
                "Premium",
                "Basic",
                "Enterprise"
            };

            var result = contracts
                .Where(c => c.Contains("Gold"))
                .ToList();

            Assert.Empty(result);
        }
    }
}