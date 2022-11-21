using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DbModels
{
	public class DbGamePlayer
	{
        [Key]
        public int gameId { get; set; } = -1;
        public int teamId { get; set; }
        public int playerId { get; set; }
        public int seasonStartYear { get; set; }
    }
}
