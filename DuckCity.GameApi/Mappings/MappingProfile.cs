using AutoMapper;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.GameApi.Dto;

namespace DuckCity.GameApi.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Player, PlayerInWaitingRoomDto>();
            CreateMap<PlayerInWaitingRoomDto, Player>();
        }
    }
}
