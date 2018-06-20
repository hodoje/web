﻿using System;
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
            // Using Driver because that class is a superset of User and includes all possible properties
            CreateMap<User, UserDto>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source => ((Gender) source.Gender).ToString()))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source => ((Role) source.Role).ToString()))
                .ForMember(destination => destination.Car,
                    opts => opts.MapFrom(source => source.Car))
                .ForMember(destination => destination.DriverLocation,
                    opts => opts.MapFrom(source => source.DriverLocation));
            CreateMap<UserDto, User>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Gender)).Cast<Gender>().SingleOrDefault(g => g.ToString() == source.Gender)))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Role)).Cast<Role>().SingleOrDefault(r => r.ToString() == source.Role)))
                .ForMember(destination => destination.Car,
                    opts => opts.MapFrom(source => source.Car))
                .ForMember(destination => destination.DriverLocation,
                    opts => opts.MapFrom(source => source.DriverLocation));
        }
    }
}