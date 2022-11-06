using Entities.Models;

namespace Entities.Types.Mappers
{
	public static class MapPositionStrToPosition
	{
        public static POSITION Map(string position)
        {
            POSITION playerPosition;
            switch (position)
            {
                case "G":
                    playerPosition = POSITION.Goalie;
                    break;

                case "L":
                    playerPosition = POSITION.LeftWing;
                    break;

                case "R":
                    playerPosition = POSITION.RightWing;
                    break;

                case "C":
                    playerPosition = POSITION.Center;
                    break;

                case "D":
                    playerPosition = POSITION.Defenseman;
                    break;

                default:
                    playerPosition = POSITION.LeftWing;
                    break;
            }

            return playerPosition;
        }
    }
}

