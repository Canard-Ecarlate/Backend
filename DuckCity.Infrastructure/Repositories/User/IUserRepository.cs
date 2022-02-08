namespace DuckCity.Infrastructure.Repositories.User
{
    public interface IUserRepository
    {
        public IList<Domain.Users.User> GetByName(string? name);

        public IList<Domain.Users.User> GetById(string? id);

        public void Create(Domain.Users.User user);

        public long CountUserByName(string? name);

        public long CountUserById(string? id);

        public long CountUserByEmail(string? email);
    }
}