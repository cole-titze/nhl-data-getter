using Entities.Types;
using Entities.Types.Mappers;

namespace Services.NhlData.Mappers
{
	public static class MapPlayerBioResponseToPositionStr
	{
		public static string Map(dynamic playerBioResponse)
		{
			return playerBioResponse.people[0].primaryPosition.abbreviation;
		}
	}
}

