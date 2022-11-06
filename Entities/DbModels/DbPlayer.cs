namespace Entities.DbModels
{
    public class DbPlayer
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public double value { get; set; }
        public int seasonStartYear { get; set; }
        public string position { get; set; } = string.Empty;
    }
}