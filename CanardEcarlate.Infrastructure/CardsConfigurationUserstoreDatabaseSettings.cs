using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Infrastructure
{
    public class CardsConfigurationUserstoreDatabaseSettings : ICardsConfigurationUserstoreDatabaseSettings
    {
        public string CardsConfigurationUsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICardsConfigurationUserstoreDatabaseSettings
    {
        string CardsConfigurationUsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
