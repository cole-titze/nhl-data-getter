namespace Services.NhlData.Mappers
{
    public static class MapTeamResponseToTeamIds
	{
		/// <summary>
		/// Maps the schedule request into a list of team ids
		/// </summary>
		/// <param name="teamResponse">Schedule nhl response</param>
		/// <returns>List of team ids</returns>
		public static List<int> Map(dynamic teamResponse)
		{
			var teamIds = new List<int>();
			foreach(var team in teamResponse.data)
			{
				int teamId = Convert.ToInt32(team.teamId);
				teamIds.Add(teamId);
			}

			return teamIds;
		}
	}
}

