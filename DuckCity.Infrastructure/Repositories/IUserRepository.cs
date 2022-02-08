using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public IList<User> GetByName(string? name);

        public IList<User> GetById(string? id);

        public void Create(User user);

        public long CountUserByName(string? name);

        public long CountUserById(string? id);

        public long CountUserByEmail(string? email);

        public void DeleteUserById(string? id);
        
        public void Replace(User user);
    }
}