using Entities.Models;
using Entities.Types;

namespace Entities.Models
{
    public class PlayerStats : IPlayerStats
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int goals { get; set; }
        public int assists { get; set; }
        public int shotsOnGoal { get; set; }
        public int blockedShots { get; set; }
        public double penaltyMinutes { get; set; }
        public int plusMinus { get; set; }
        public double faceoffPercent { get; set; }
        public int gamesPlayed { get; set; }
        // Default to stop null warning left-wing has no special rules
        public POSITION position { get; set; } = POSITION.LeftWing;

        // Player Game Score = ( (0.75 * G) + (.63A) + (0.075 * SOG) + (0.05 * BLK) – (0.075 * PIM) + (0.15 * PM) + ((17*GP*FOP)*.01) ) / GP
        public double GetPlayerValue()
        {
            if (gamesPlayed < 5)
                return 0;

            var value = ((.75 * goals) + (.63 * assists) + (.075 * shotsOnGoal) + (.05 * blockedShots) - (.075 * penaltyMinutes)
                + (.15 * plusMinus)) / gamesPlayed;

            // Ignore faceoffs unless player is a center
            if (position == POSITION.Center)
                value += 17 * faceoffPercent * .01;

            return Math.Max(value * 60, 0);
        }
    }
}
