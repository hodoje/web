using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Backend.DataAccess;
using Backend.DataAccess.ModelRepositories;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class DefaultController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DefaultController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Location dl = new Location
            {
                City = "NS",
                StreetName = "STR",
                StreetNumber = "123",
                PostalCode = "ABC123",
                Longitude = 5,
                Latitude = 7
            };
            _unitOfWork.LocationRepository.Add(dl);
            _unitOfWork.Complete();

            User d = new User
            {
                Username = "user",
                Password = "pass",
                Name = "nikola",
                Lastname = "karaklic",
                DriverLocationId = 1,
                Email = "email",
                Gender = 1,
                IsBanned = false,
                NationalIdentificationNumber = "1234",
                PhoneNumber = "12345",
                Role = 2,
                Car = new Car { CarType = 2, RegistrationNumber = "ABC123", YearOfManufactoring = 1996, TaxiNumber = "44" }
            };
            _unitOfWork.UserRepository.Add(d);
            _unitOfWork.Complete();
        }

        public User GetDefault()
        {
            User u = _unitOfWork.UserRepository.Find(x => x.IsBanned == false).First();
            u.DriverLocation = _unitOfWork.LocationRepository.Find(l => l.Drivers.FirstOrDefault(user => user.Id == u.Id) != null).First();           
            u.Comments = _unitOfWork.CommentRepository.Find(c => c.UserId == u.Id).ToList();
            u.CustomerRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            u.DriverRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            u.DispatcherRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            return u;
        }
    }
}
