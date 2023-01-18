using Entities.Models;

namespace Entities.Types.Mappers
{
	public static class MapPositionToPositionStr
	{
        public static string Map(POSITION position)
        {
            string positionStr = "L";
            switch (position)
            {
                case POSITION.Goalie:
                    positionStr = "G";
                    break;

                case POSITION.LeftWing:
                    positionStr = "L";
                    break;

                case POSITION.RightWing:
                    positionStr = "R";
                    break;

                case POSITION.Center:
                    positionStr = "C";
                    break;

                case POSITION.Defenseman:
                    positionStr = "D";
                    break;
            }

            return positionStr;
        }
    }
}

