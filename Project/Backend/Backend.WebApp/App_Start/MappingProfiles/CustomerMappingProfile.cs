using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, UserDto>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source => ((Gender)source.Gender).ToString()))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source => ((Role)source.Role).ToString()))
                .ForMember(destination => destination.Car,
                    opts => opts.Ignore())
                .ForMember(destination => destination.DriverLocation,
                    opts => opts.Ignore());
            CreateMap<UserDto, Customer>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Gender)).Cast<Gender>().SingleOrDefault(g => g.ToString() == source.Gender)))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Role)).Cast<Role>().SingleOrDefault(r => r.ToString() == source.Role)));
            CreateMap<Customer, CustomerDto>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source => ((Gender) source.Gender).ToString()))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source => ((Role) source.Role).ToString()));
            CreateMap<CustomerDto, Customer>()
                .ForMember(destination => destination.Gender,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Gender)).Cast<Gender>().SingleOrDefault(g => g.ToString() == source.Gender)))
                .ForMember(destination => destination.Role,
                    opts => opts.MapFrom(source =>
                        Enum.GetValues(typeof(Role)).Cast<Role>().SingleOrDefault(r => r.ToString() == source.Role)));
        }
    }
}