using System.ComponentModel.DataAnnotations;

namespace Entities.DbModels
{
    public class DbSeasonGameCount
    {
        [Key]
        public int seasonId { get; set; }
        public int gameCount { get; set; }
    }
}