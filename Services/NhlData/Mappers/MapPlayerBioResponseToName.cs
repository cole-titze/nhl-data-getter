namespace Services.NhlData.Mappers
{
    public static class MapPlayerBioResponseToName
	{
		/// <summary>
		/// Gets the name of a player from the player bio response
		/// </summary>
		/// <param name="playerBioResponse">The player bio response</param>
		/// <returns>The players name</returns>
		public static string Map(dynamic playerBioResponse)
		{
            return (string)playerBioResponse.people[0].fullName;
        }
	}
}
