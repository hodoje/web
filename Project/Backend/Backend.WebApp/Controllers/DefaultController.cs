using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Backend.DataAccess;
using Backend.DataAccess.ModelRepositories;
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
            //Address a = new Address() { Id = 1, City = "NS", StreetName = "str", StreetNumber = "5", PostalCode = "123" };
            //_unitOfWork.AddressRepository.Add(a);
            //_unitOfWork.Complete();
            //Location dl = new Location() { Id = 1, AddressId = a.Id, CoordinateX = 5, CoordinateY = 7 };
            //_unitOfWork.LocationRepository.Add(dl);
            //_unitOfWork.Complete();
            //Gender g = new Gender() { Id = 1, GenderName = "male" };
            //_unitOfWork.GenderRepository.Add(g);
            //_unitOfWork.Complete();
            //Role ro = new Role() { Id = 1, RoleName = "driver" };
            //_unitOfWork.RoleRepository.Add(ro);
            //_unitOfWork.Complete();
            //Role roo = new Role() { Id = 2, RoleName = "customer" };
            //_unitOfWork.RoleRepository.Add(ro);
            //_unitOfWork.Complete();
            //CarType ct = new CarType() { Id = 1, TypeName = "passanger" };
            //_unitOfWork.CarTypeRepository.Add(ct);
            //_unitOfWork.Complete();
            //RideStatus rs = new RideStatus() {Id = 1, StatusName = "status1"};
            //_unitOfWork.RideStatusRepository.Add(rs);
            //_unitOfWork.Complete();

            //Driver d = new Driver();
            //d.Username = "user";
            //d.Password = "pass";
            //d.Name = "nikola";
            //d.Lastname = "karaklic";
            //d.Car = new Car() { Id = 1, CarTypeId = ct.Id, RegistrationNumber = "123", TaxiNumber = "22", YearOfManufactoring = 636 };
            //d.CarId = 1;
            //d.DriverLocationId = 1;
            //d.Comments = new List<Comment>();
            //d.DriverRides = new List<Ride>();
            //d.Email = "email";
            //d.GenderId = 1;
            //d.IsBanned = false;
            //d.NationalIdentificationNumber = "1234";
            //d.PhoneNumber = "12345";            
            //d.RoleId = 1;
            //_unitOfWork.UserRepository.Add(d);
            //_unitOfWork.Complete();
        }

        public User GetDefault()
        {
            Driver u = (Driver)_unitOfWork.UserRepository.Find(x => x.IsBanned == false).First();
            u.Gender = _unitOfWork.GenderRepository.Find(g => g.Users.FirstOrDefault(user => user.Id == u.Id) != null).First();
            u.Car = _unitOfWork.CarRepository.Find(c => c.Driver.Id == u.Id).First();
            u.Car.CarType = _unitOfWork.CarTypeRepository.Find(ct => ct.Cars.FirstOrDefault(c => c.Id == u.CarId) != null).First();
            u.Role = _unitOfWork.RoleRepository.Find(r => r.Users.FirstOrDefault(user => user.Id == u.Id) != null).First();
            u.DriverLocation = _unitOfWork.LocationRepository.Find(l => l.Drivers.FirstOrDefault(user => user.Id == u.Id) != null).First();
            u.DriverLocation.Address = _unitOfWork.AddressRepository.Find(a => a.Id == u.DriverLocation.AddressId).First();
            u.Comments = _unitOfWork.CommentRepository.Find(c => c.UserId == u.Id).ToList();
            u.CustomerRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            u.DriverRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            u.DispatcherRides = _unitOfWork.RideRepository.Find(r => r.DriverId == u.Id).ToList();
            return u;
        }
    }
}
