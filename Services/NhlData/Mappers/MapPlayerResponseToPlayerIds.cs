using Entities.DbModels;

namespace Services.NhlData.Mappers
{
	public static class MapPlayerResponseToPlayerIds
	{
		/// <summary>
		/// Maps the player response to a list of player ids
		/// </summary>
		/// <param name="playerResponse">Nhl response that contains a teams roster</param>
		/// <returns></returns>
		public static List<int> Map(dynamic playerResponse)
		{
			var playerIds = new List<int>();
			int playerId;
			foreach(var player in playerResponse.roster)
			{
				playerId = Convert.ToInt32(player.person.id);
				playerIds.Add(playerId);
			}

			return playerIds;
		}
	}
}

