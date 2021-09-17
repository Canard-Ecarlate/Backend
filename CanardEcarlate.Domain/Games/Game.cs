using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanardEcarlate.Domain.Database
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string HostName { get; set; }
        public GameConfiguration GameConfiguration { get; set; }
        public List<Player> Players { get; set; }
        public InGameData InGameData { get; set; }
    }
}
