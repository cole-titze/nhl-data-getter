using System;
namespace Services.NhlData.Mappers
{
	public static class MapPlayerBioResponseToName
	{
		public static string Map(dynamic playerBioResponse)
		{
			string name = (string)playerBioResponse.people[0].fullName;
			return name;
        }
	}
}

