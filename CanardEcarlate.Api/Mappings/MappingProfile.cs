using AutoMapper;
using CanardEcarlate.Api.Models;
using CanardEcarlate.Api.Models.Authentication;
using CanardEcarlate.Domain;

namespace CanardEcarlate.Api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, UserWithToken>();
            CreateMap<UserWithToken, User>();
        }
    }
}
