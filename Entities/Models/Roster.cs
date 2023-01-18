using Entities.DbModels;

namespace Entities.Models
{
    public class Roster
	{
		public List<DbGamePlayer> homeTeam { get; set; } = new List<DbGamePlayer>();
        public List<DbGamePlayer> awayTeam { get; set; } = new List<DbGamePlayer>();
    }
}

