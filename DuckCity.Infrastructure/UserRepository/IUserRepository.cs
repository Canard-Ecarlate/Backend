using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.UserRepository;

public interface IUserRepository
{
    public void Create(User user);
        
    public IList<User> FindByName(string? name);

    public long CountUserByName(string? name);

    public long CountUserById(string? id);

    public long CountUserByEmail(string? email);
}