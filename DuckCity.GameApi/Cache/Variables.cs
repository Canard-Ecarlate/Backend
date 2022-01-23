using System.Collections.ObjectModel;
using DuckCity.Domain.Games;

namespace DuckCity.GameApi.Cache
{
    public static class Variables
    {
        public static Collection<Game> Games { get; } = new();
    }
}


