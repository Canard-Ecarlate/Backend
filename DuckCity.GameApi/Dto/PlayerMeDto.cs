using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.GameApi.Dto
{
    public class PlayerMeDto
    {
        public IRole? Role { get; set; }
        public List<ICard>? CardsInHand { get; set; }
    }
}