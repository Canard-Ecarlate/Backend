using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.UserRepository;

public interface IUserRepository
{
    public void Create(User user);
        
    public User FindByName(string? name);
    
    public User FindById(string? id);

    public long CountUserByName(string? name);

    public long CountUserById(string? id);

    public long CountUserByEmail(string? email);
    
    public void DeleteUserById(string? id);
        
    public void Replace(User user);
}