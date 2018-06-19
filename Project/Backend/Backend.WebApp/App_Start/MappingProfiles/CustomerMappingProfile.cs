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
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}