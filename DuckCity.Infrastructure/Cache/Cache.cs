using System.Collections.ObjectModel;
using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Cache
{
    public static class Cache
    {
        public static Collection<Game> Games { get; } = new();
    }
}