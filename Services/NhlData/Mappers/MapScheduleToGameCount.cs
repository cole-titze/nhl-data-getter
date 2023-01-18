namespace Services.NhlData.Mappers
{
    public static class MapScheduleToGameCount
    {
        /// <summary>
        /// Gets the number of games from a schedule response
        /// </summary>
        /// <param name="scheduleResponse">Response from Nhl api</param>
        /// <returns>Number of games in the season</returns>
        public static int Map(dynamic scheduleResponse)
        {
            return scheduleResponse.SeasonGameCount;
        }
    }
}

