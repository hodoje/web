using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class DispatcherMappingProfile : Profile
    {
        public DispatcherMappingProfile()
        {
            CreateMap<Dispatcher, DispatcherDto>();
            CreateMap<DispatcherDto, Dispatcher>();
        }
    }
}