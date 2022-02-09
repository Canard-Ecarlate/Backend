using DuckCity.Domain;

namespace DuckCity.Infrastructure.GameContainerRepository;

public interface IGameContainerRepository
{
    void Create(GameContainer gameContainer);
    GameContainer FindById(string gameContainerId);
    GameContainer FindByNumber(int number);
    void Update(GameContainer gameContainer);
    void Delete(string gameContainerId);
}