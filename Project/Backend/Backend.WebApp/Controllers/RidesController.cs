using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Backend.DataAccess;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;
using DomainEntities.Models;
using Backend.AccessServices;
using Backend.Dtos;

namespace Backend.Controllers
{
    public class RidesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IAccessService _accessService;
        private IMapper _iMapper;

        public RidesController(IUnitOfWork unitOfWork, IAccessService accessService, IMapper iMapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _iMapper = iMapper;
        }


        [HttpPost]
        [Route("api/rides/rideRequest")]
        public IHttpActionResult RequestRide(ApiMessage<string, RideRequestModel> rideRequestApiMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Location newRideLocation = _iMapper.Map<LocationDto, Location>(rideRequestApiMessage.Data.Location);

            try
            {
                _unitOfWork.LocationRepository.Add(newRideLocation);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(ModelState);
            }

            LoginModel loginModel = _accessService.GetLoginData(rideRequestApiMessage.Key, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);

            Ride newRide = new Ride();
            newRide.StartLocationId = newRideLocation.Id;
            newRide.CarType = (int)Enum.GetValues(typeof(CarType)).Cast<CarType>().FirstOrDefault(ct => ct.ToString() == rideRequestApiMessage.Data.CarType);
            newRide.CustomerId = user.Id;

            try
            {
                _unitOfWork.RideRepository.Add(newRide);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(ModelState);
            }
            
            RideDto newRideDto = _iMapper.Map<Ride, RideDto>(newRide);

            return CreatedAtRoute("DefaultApi", new { controller = "rides", id = newRideDto.Id }, newRideDto);
        }

        [HttpPost]
        [Route("api/rides/changeRideRequest")]
        public IHttpActionResult ChangeRideRequest(ApiMessage<string, ChangeRideRequestModel> changeRideRequestApiMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginModel loginModel = _accessService.GetLoginData(changeRideRequestApiMessage.Key, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);
            Ride oldRide = _unitOfWork.RideRepository.Find(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
            oldRide.StartLocation = _unitOfWork.LocationRepository.GetById(oldRide.StartLocationId);
            Ride updatedRide = new Ride()
            {
                StartLocation = _iMapper.Map<LocationDto, Location>(changeRideRequestApiMessage.Data.Location),
                CarType = (int) Enum.GetValues(typeof(CarType)).Cast<CarType>().FirstOrDefault(t => t.ToString() == changeRideRequestApiMessage.Data.CarType)
            };

            oldRide.StartLocation.Address = updatedRide.StartLocation.Address;
            oldRide.StartLocation.Longitude = updatedRide.StartLocation.Longitude;
            oldRide.StartLocation.Latitude = updatedRide.StartLocation.Latitude;
            oldRide.CarType = updatedRide.CarType;

            try
            {
                _unitOfWork.RideRepository.Update(oldRide);
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/rides/cancelRideRequest")]
        public IHttpActionResult CancelRideRequest(ApiMessage<string, CancelRideRequestModel> cancelRideRequestApiMessage)
        {
            LoginModel loginModel = _accessService.GetLoginData(cancelRideRequestApiMessage.Key, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);
            Ride oldRide = _unitOfWork.RideRepository.Find(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
            oldRide.RideStatus = (int) RideStatus.CANCELLED;
            _unitOfWork.RideRepository.Update(oldRide);
            _unitOfWork.Complete();
            return Ok(oldRide);
        }
    }
}