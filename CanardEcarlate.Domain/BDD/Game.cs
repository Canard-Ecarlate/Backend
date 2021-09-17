using CanardEcarlate.Domain.Cards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.BDD
{
    public class Game
    {
        [Required]
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
