using AutoMapper;
using CanardEcarlate.Domain;

namespace CanardEcarlate.Api.Models
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, UserWithToken>();
            CreateMap<UserWithToken, User>();
        }
    }
}