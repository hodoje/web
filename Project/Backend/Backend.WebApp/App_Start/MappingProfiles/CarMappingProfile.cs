using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDto>()
                .ForMember(destination => destination.Driver,
                    opts => opts.MapFrom(source => source.Driver))
                .ForMember(destination => destination.CarType,
                    opts => opts.MapFrom(source => ((CarType) source.CarType).ToString()));
            CreateMap<CarDto, Car>()
                .ForMember(destination => destination.Driver,
                    opts => opts.MapFrom(source => source.Driver))
                .ForMember(destination => destination.CarType,
                    opts => opts.MapFrom(source => Enum.GetValues(typeof(CarType)).Cast<CarType>().Select(ct => ct.ToString() == source.CarType)));
        }
    }
}