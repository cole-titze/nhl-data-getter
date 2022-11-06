namespace Entities.Types
{
    public class YearRange
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public YearRange(int startYear, int endYear)
        {
            StartYear = startYear;
            EndYear = endYear;
        }
        public YearRange(int startYear, DateTime endDate)
        {
            StartYear = startYear;
            EndYear = GetEndSeason(endDate);
        }
        /// <summary>
        /// Season spans 2 years (2021-2022) but we only want the start year of the season
        // (ex. February 2022 we want 2021 to be the end season)
        /// </summary>
        /// <param name="currentDate">The date to translate into a year</param>
        /// <returns>The current season start year by date</returns>
        public int GetEndSeason(DateTime date)
        {
            var endSeasonDate = new DateTime(date.Year, 09, 15);
            int currentSeasonYear;

            if (date > endSeasonDate)
                currentSeasonYear = date.Year;
            else
                currentSeasonYear = date.Year - 1;

            return currentSeasonYear;
        }
    }
}
