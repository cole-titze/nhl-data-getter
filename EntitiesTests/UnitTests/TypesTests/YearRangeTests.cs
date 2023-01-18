using Entities.Types;
using FluentAssertions;

namespace EntitiesTests.UnitTests.TypesTests
{
    [TestClass]
    public class YearRangeTests
    {
        [TestMethod]
        public void BuildYearRange_WithEarlySeasonDate_ShouldHaveGivenYearAsEndYear()
        {
            int startYear = 2011;
            DateTime endDate = DateTime.Parse("11/1/2020");
            int expectedEndYear = endDate.Year;
            var yearRange = new YearRange(2011, endDate);

            yearRange.StartYear.Should().Be(startYear);
            yearRange.EndYear.Should().Be(expectedEndYear);
        }
        [TestMethod]
        public void BuildYearRange_WithLateSeasonDate_ShouldHavePreviousYearAsEndYear()
        {
            int startYear = 2011;
            DateTime endDate = DateTime.Parse("2/1/2021");
            int expectedEndYear = endDate.Year - 1;
            var yearRange = new YearRange(2011, endDate);

            yearRange.StartYear.Should().Be(startYear);
            yearRange.EndYear.Should().Be(expectedEndYear);
        }
        [TestMethod]
        public void BuildYearRange_WithGivenYears_ShouldHaveCorrectRange()
        {
            int startYear = 2011;
            int endYear = 2022;
            var yearRange = new YearRange(startYear, endYear);

            yearRange.StartYear.Should().Be(startYear);
            yearRange.EndYear.Should().Be(endYear);
        }
    }
}
