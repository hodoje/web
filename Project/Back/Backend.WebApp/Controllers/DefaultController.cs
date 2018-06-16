using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.DataAccess;
using Backend.DataAccess.TypeModelRepositories;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;
using DomainEntities.EnumDatabaseModels;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class DefaultController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DefaultController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //Customer u = new Customer();
            //u.Comments = new List<Comment>();
            //u.CustomerRides = new List<Ride>();
            //u.DispatcherRides = new List<Ride>();
            //u.DriverRides = new List<Ride>();
            //u.Email = "email";
            //u.Gender = new Gender { GenderName = "male" };
            //u.IsBanned = false;
            //u.GenderId = u.Gender.Id;
            //u.Lastname = "karaklic";
            //u.Name = "nikola";
            //u.NationalIdentificationNumber = "1234";
            //u.Password = "123";
            //u.PhoneNumber = "062123";
            //u.Role = new Role {RoleName = "Customer", Users = new List<User>()};
            //u.RoleId = u.Role.Id;
            //u.Username = "hodoje";
            //_unitOfWork.GenderRepository.Add(u.Gender);
            //_unitOfWork.Complete();
            //_unitOfWork.RoleRepository.Add(u.Role);
            //_unitOfWork.Complete();
            //_unitOfWork.UserRepository.Add(u);
            //_unitOfWork.Complete();
        }

        public Default GetDefault()
        {
            return new Default {DefaultName = "Nikola", DefaultLastName = "Karaklic"};
        }
    }
}
