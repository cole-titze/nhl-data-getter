using System;
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
			string name = (string)playerBioResponse.people[0].fullName;
			return name;
        }
	}
}
