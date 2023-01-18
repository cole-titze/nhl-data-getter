namespace Entities.DbModels
{
    public class DbGamePlayer
	{
        public int gameId { get; set; }
        public int teamId { get; set; }
        public int playerId { get; set; }
        public int seasonStartYear { get; set; }
    }
}
