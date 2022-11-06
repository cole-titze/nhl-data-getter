using Entities.Types;

namespace Entities.Models
{
    public interface IPlayerStats
    {
        public int id { get; set; }
        public string name { get; set; }
        public POSITION position { get; set; }
        public double GetPlayerValue();
    }
}