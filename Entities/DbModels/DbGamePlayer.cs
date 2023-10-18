namespace Entities.DbModels
{
    public class DbGamePlayer
	{
        public int gameId { get; set; }
        public int teamId { get; set; }
        public int playerId { get; set; }
        public int seasonStartYear { get; set; }
        public void Clone(DbGamePlayer player)
        {
            gameId = player.gameId;
            teamId = player.teamId;
            playerId = player.playerId;
            seasonStartYear = player.seasonStartYear;
        }
        public void Clone(DbGamePlayer player, int gameId)
        {
            Clone(player);
            player.gameId = gameId;
        }
    }
}
