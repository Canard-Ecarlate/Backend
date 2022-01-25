using AutoMapper;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Domain;
using DuckCity.Domain.Users;

namespace DuckCity.Api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, UserWithTokenDto>();
            CreateMap<UserWithTokenDto, User>();
        }
    }
}
