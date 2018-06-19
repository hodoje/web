using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source => ((Gender) source.Gender).ToString()))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source => ((Role) source.Role).ToString()));                
            CreateMap<UserDto, User>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(g => g.ToString() == source.Gender)))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Role)).Cast<Role>().Select(r => r.ToString() == source.Role)));
        }
    }
}