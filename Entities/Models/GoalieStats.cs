namespace Entities.Models
{
    public class GoalieStats : IPlayerStats
	{
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int goalsAgainst { get; set; }
        public int saves { get; set; }
        public int gamesStarted { get; set; }

        // Default to goalie since this is goalie class
        public POSITION position { get; set; } = POSITION.Goalie;

        // Goalie Game Score = ( (-0.75 * GA) + (0.1 * SV) ) / GP
        public double GetPlayerValue()
        {
            if (gamesStarted < 5)
                return 0;

            var value = ((-.75 * goalsAgainst) + (.1 * saves)) / gamesStarted;

            return Math.Max(value * 60, 0);
        }
    }
}

