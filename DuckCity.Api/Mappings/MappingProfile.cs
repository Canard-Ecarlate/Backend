﻿using AutoMapper;
using DuckCity.Api.Models.Authentication;
using DuckCity.Domain;

namespace DuckCity.Api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, UserWithToken>();
            CreateMap<UserWithToken, User>();
        }
    }
}