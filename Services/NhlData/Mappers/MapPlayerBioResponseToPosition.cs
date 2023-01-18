namespace Services.NhlData.Mappers
{
    public static class MapPlayerBioResponseToPositionStr
	{
		/// <summary>
		/// Gets the position of a player from the nhl response
		/// </summary>
		/// <param name="playerBioResponse">The nhl api response containing the player bio</param>
		/// <returns>The position of the player</returns>
		public static string Map(dynamic playerBioResponse)
		{
			return playerBioResponse.people[0].primaryPosition.abbreviation;
        }
	}
}

