using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain
{
    public class CardsConfigurationStatistics
    {
        [Key]
        public int Id { get; set; }
        public CardsConfiguration CardsConfiguration { get; set; }
        public int NbWonAsCIAT { get; set; }
        public int NbWonAsCE { get; set; }
    }
}
