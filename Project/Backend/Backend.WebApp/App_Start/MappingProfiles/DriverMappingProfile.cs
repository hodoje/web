using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<Driver, DriverDto>()
                .ForMember(destination => destination.Car,
                    opts => opts.MapFrom(source => source.Car))
                .ForMember(destination => destination.DriverLocation,
                    opts => opts.MapFrom(source => source.DriverLocation));
            CreateMap<DriverDto, Driver>()
                .ForMember(destination => destination.Car,
                    opts => opts.MapFrom(source => source.Car))
                .ForMember(destination => destination.DriverLocation,
                    opts => opts.MapFrom(source => source.DriverLocation));
        }
    }
}