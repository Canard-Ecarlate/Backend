using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.UserRepository
{
    public interface IUserRepository
    {
        public IList<User> GetByName(string? name);

        public IList<User> GetById(string? id);

        public void Create(User user);

        public long CountUserByName(string? name);

        public long CountUserById(string? id);

        public long CountUserByEmail(string? email);
    }
}