using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Infrastructure
{
    public class GamestoreDatabaseSettings : IGamestoreDatabaseSettings
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IGamestoreDatabaseSettings
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
