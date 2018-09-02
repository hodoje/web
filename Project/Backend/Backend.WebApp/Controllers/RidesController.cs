﻿using System;
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
using Newtonsoft.Json;

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

        [HttpGet]
        [Route("api/rides/getAllRides")]
        [ResponseType(typeof(IEnumerable<RideDto>))]
        public IHttpActionResult GetAllRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                List<Ride> allRides = _unitOfWork.RideRepository.GetAllRidesIncludeAll().ToList();
                foreach (Ride r in allRides)
                {
                    r.Comments = _unitOfWork.CommentRepository.FindAllCommentsIncludeUser(c => c.RideId == r.Id).ToList();
                }
                List<RideDto> allRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allRides);
                return Ok(allRidesDtos);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/rides/getAllMyRides")]
        [ResponseType(typeof(IEnumerable<RideDto>))]
        public IHttpActionResult GetAllMyRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                List<Ride> allUserRides = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.Customer.Username == loginModel.Username).ToList();
                foreach (Ride r in allUserRides)
                {
                    r.Comments = _unitOfWork.CommentRepository.FindAllCommentsIncludeUser(c => c.RideId == r.Id).ToList();
                }
                List<RideDto> allUserRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allUserRides);
                return Ok(allUserRidesDtos);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/rides/getAllDispatcherRides")]
        public IHttpActionResult GetAllDispatcherRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                User dispatcher = _unitOfWork.UserRepository.Find(u => u.Username == loginModel.Username).FirstOrDefault();
                List<Ride> allDispatcherRides = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.DispatcherId == dispatcher.Id).ToList();
                foreach (Ride r in allDispatcherRides)
                {
                    r.Comments = _unitOfWork.CommentRepository.FindAllCommentsIncludeUser(c => c.RideId == r.Id).ToList();
                }
                List<RideDto> allDispatcherRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allDispatcherRides);
                return Ok(allDispatcherRidesDtos);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/rides/getAllDriverRides")]
        public IHttpActionResult GetAllDriverRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                User driver = _unitOfWork.UserRepository.Find(u => u.Username == loginModel.Username).FirstOrDefault();
                List<Ride> allDriverRides = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.DriverId == driver.Id).ToList();
                foreach (Ride r in allDriverRides)
                {
                    r.Comments = _unitOfWork.CommentRepository.FindAllCommentsIncludeUser(c => c.RideId == r.Id).ToList();
                }
                List<RideDto> allDriverRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allDriverRides);
                return Ok(allDriverRidesDtos);
            }
            return BadRequest();
        }
        

        [HttpGet]
        [Route("api/rides/getAllPendingRides")]
        public IHttpActionResult GetAllPendingRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                User dispatcher = _unitOfWork.UserRepository.Find(u => u.Username == loginModel.Username).FirstOrDefault();
                List<Ride> allPendingRides = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.RideStatus == (int)RideStatus.CREATED).ToList();
                foreach (Ride r in allPendingRides)
                {
                    r.Comments = _unitOfWork.CommentRepository.FindAllCommentsIncludeUser(c => c.RideId == r.Id).ToList();
                }
                List<RideDto> allPendingRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allPendingRides);
                return Ok(allPendingRidesDtos);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("api/rides/rideRequest")]
        public IHttpActionResult RequestRide(ApiMessage<string, RideRequestModel> rideRequestApiMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginModel loginModel = _accessService.GetLoginData(rideRequestApiMessage.Key, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);

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

            Ride newRide = new Ride();
            newRide.StartLocationId = newRideLocation.Id;
            newRide.CarType = (int)Enum.GetValues(typeof(CarType)).Cast<CarType>().FirstOrDefault(ct => ct.ToString() == rideRequestApiMessage.Data.CarType);
            newRide.CustomerId = user.Id;
            newRide.Timestamp = DateTime.Now;

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
            Ride oldRide = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
            //oldRide.StartLocation = _unitOfWork.LocationRepository.GetById(oldRide.StartLocationId);
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

            RideDto oldRideDto = _iMapper.Map<Ride, RideDto>(oldRide);
            return Ok(oldRideDto);
        }

        [HttpPost]
        [Route("api/rides/cancelRideRequest")]
        public IHttpActionResult CancelRideRequest(ApiMessage<string, CancelRideRequestModel> cancelRideRequestApiMessage)
        {
            LoginModel loginModel = _accessService.GetLoginData(cancelRideRequestApiMessage.Key, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);
            Ride oldRide = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
            if (oldRide != null)
            {
                oldRide.RideStatus = (int)RideStatus.CANCELLED;
                _unitOfWork.RideRepository.Update(oldRide);
                _unitOfWork.Complete();
                RideDto oldRideDto = _iMapper.Map<Ride, RideDto>(oldRide);
                return Ok(oldRideDto);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("api/rides/commentLatestCancelledRide")]
        public IHttpActionResult CommentLatestCancelledRide(CommentDto commentDto)
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);

            LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);

            List<Ride> allUserRides = _unitOfWork.RideRepository.GetAllRidesIncludeAll().ToList();
            allUserRides = allUserRides.OrderByDescending(r => r.Timestamp).ToList(); //newest first
            Ride latestRide = allUserRides.FirstOrDefault(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CANCELLED);
            if (latestRide != null)
            {
                Comment comment;
                if ((comment = latestRide.Comments.FirstOrDefault(c => c.UserId == user.Id)) == null)
                {
                    comment = _iMapper.Map<CommentDto, Comment>(commentDto);
                    comment.UserId = user.Id;
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : comment.Description;
                    comment.Timestamp = DateTime.Now;
                    latestRide.Comments.Add(comment);
                    _unitOfWork.Complete();
                }
                else
                {
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : commentDto.Description;
                    comment.Timestamp = DateTime.Now;
                    _unitOfWork.CommentRepository.Update(comment);
                    _unitOfWork.Complete();
                }
            }

            RideDto rideDto = _iMapper.Map<Ride, RideDto>(latestRide);
            return Ok(rideDto);
        }

        [HttpPost]
        [Route("api/rides/addComment")]
        public IHttpActionResult AddCommentForARide(CommentDto commentDto)
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);

            LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);

            Ride ride = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.Id == commentDto.RideId).FirstOrDefault();
            if (ride != null)
            {
                Comment comment;
                if ((comment = ride.Comments.FirstOrDefault(c => c.UserId == user.Id)) == null)
                {
                    comment = _iMapper.Map<CommentDto, Comment>(commentDto);
                    comment.UserId = user.Id;
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : comment.Description;
                    comment.Timestamp = DateTime.Now;
                    ride.Comments.Add(comment);
                    _unitOfWork.Complete();
                }
                else
                {
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : commentDto.Description;
                    comment.Timestamp = DateTime.Now;
                    _unitOfWork.CommentRepository.Update(comment);
                    _unitOfWork.Complete();
                }
            }

            RideDto rideDto = _iMapper.Map<Ride, RideDto>(ride);
            return Ok(rideDto);
        }

        [HttpPost]
        [Route("api/rides/rateARide")]
        public IHttpActionResult RateARide(CommentDto commentDto)
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);

            LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
            Comment comment = _unitOfWork.CommentRepository.GetById(commentDto.Id);
            comment.Rating = commentDto.Rating;
            _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Complete();
            return Ok();
        }

        // Here we will encounter some magic strings...
        // And some so poorly optimized code, like you can get fired for doing this...
        [HttpPost]
        [Route("api/rides/refine")]
        public IHttpActionResult RefineRides(RefineRidesModel refine)
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            LoginModel loginModel = null;
            if (_accessService.IsLoggedIn(hash))
            {
                if (_accessService.GetLoginData(hash, _unitOfWork) != null)
                {
                    loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                }
            }
            User user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);

            List<Ride> refinedRides = new List<Ride>();
            if (String.IsNullOrWhiteSpace(refine.Filter))
            {
                // NO FILTER

                bool shouldDateSearch = (refine.Search.ByDate.From != null || refine.Search.ByDate.To != null);
                bool shouldRatingSearch = (refine.Search.ByRating.From != null || refine.Search.ByRating.To != null);
                bool shouldPriceSearch = (refine.Search.ByPrice.From != null || refine.Search.ByPrice.To != null);

                List<Ride> distinctList = new List<Ride>();

                if (shouldDateSearch)
                {
                    //BY DATE
                    if ((refine.Search.ByDate.From == null && refine.Search.ByDate.To == null) ||
                    (refine.Search.ByDate.From == DateTime.MinValue && refine.Search.ByDate.To == DateTime.MinValue))
                    {
                        distinctList =
                            (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByDate.From == null || refine.Search.ByDate.From == DateTime.MinValue)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Timestamp <= refine.Search.ByDate.To);
                        }
                        else if (refine.Search.ByDate.To == null || refine.Search.ByDate.To == DateTime.MinValue)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Timestamp >= refine.Search.ByDate.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Timestamp >= refine.Search.ByDate.From &&
                                                                                                                     r.Timestamp <= refine.Search.ByDate.To);
                        }
                    }
                }

                if (shouldRatingSearch)
                {
                    // BY RATING              
                    if (refine.Search.ByRating.From == null && refine.Search.ByRating.To == null)
                    {
                        distinctList =
                            (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByRating.From == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating <= refine.Search.ByRating.To);
                        }
                        else if (refine.Search.ByRating.To == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating >= refine.Search.ByRating.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating >= refine.Search.ByRating.From &&
                                                                                                                      r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating <= refine.Search.ByRating.To);
                        }
                    }
                }

                if (shouldPriceSearch)
                {
                    // BY PRICE
                    if (refine.Search.ByPrice.From == null && refine.Search.ByPrice.To == null)
                    {
                        distinctList =
                            (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByPrice.From == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price <= refine.Search.ByPrice.To);
                        }
                        else if (refine.Search.ByRating.To == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price >= refine.Search.ByPrice.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price >= refine.Search.ByPrice.From &&
                                                                                                                     r.Price <= refine.Search.ByPrice.To);
                        }
                    }
                }

                if (!shouldDateSearch && !shouldRatingSearch && !shouldRatingSearch)
                {
                    distinctList = _unitOfWork.RideRepository.GetAllRidesIncludeAll().ToList();
                }

                distinctList = distinctList.GroupBy(r => r.Id).Select(group => group.FirstOrDefault()).ToList();

                refinedRides = SortList(refine.Sort, distinctList);
            }
            else
            {
                // FILTER
                List<Ride> filteredList = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.CustomerId == user.Id && 
                                                                                                                   ((RideStatus) r.RideStatus).ToString() == refine.Filter).ToList();
                refinedRides = SortList(refine.Sort, filteredList);
            }
            
            List<RideDto> refinedRideDtos = _iMapper.Map<List<Ride>, List<RideDto>>(refinedRides);
            return Ok(refinedRideDtos);
        }

        private List<Ride> SortList(SortRidesModel sortCriteria, List<Ride> unsortedList)
        {
            List<Ride> sortedList = new List<Ride>();
            if (String.IsNullOrWhiteSpace(sortCriteria.ByDate))
            {
                if (String.IsNullOrWhiteSpace(sortCriteria.ByRating))
                {
                    sortedList = unsortedList.ToList();
                }
                else if (sortCriteria.ByRating == "HIGHEST")
                {
                    sortedList = unsortedList.OrderByDescending(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                }
                else if (sortCriteria.ByRating == "LOWEST")
                {
                    sortedList = unsortedList.OrderBy(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                    //sortedList = unsortedList.OrderBy(r =>
                    //{
                    //    double zbir = 0;
                    //    double prosek = 0;
                    //    foreach (Comment c in r.Comments)
                    //    {
                    //        zbir += c.Rating;
                    //    }
                    //    prosek = zbir / r.Comments.Count;
                    //    return prosek;
                    //}).ToList();
                }
            }
            else
            {
                if (sortCriteria.ByDate == "OLDEST")
                {
                    if (String.IsNullOrWhiteSpace(sortCriteria.ByRating))
                    {
                        sortedList = unsortedList.OrderBy(r => r.Timestamp).ToList();
                    }
                    else if (sortCriteria.ByRating == "HIGHEST")
                    {
                        sortedList = unsortedList.OrderBy(r => r.Timestamp).ThenByDescending(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                    }
                    else if (sortCriteria.ByRating == "LOWEST")
                    {
                        sortedList = unsortedList.OrderBy(r => r.Timestamp).ThenBy(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                    }
                }
                else if (sortCriteria.ByDate == "NEWEST")
                {
                    if (String.IsNullOrWhiteSpace(sortCriteria.ByRating))
                    {
                        sortedList = unsortedList.OrderByDescending(r => r.Timestamp).ToList();
                    }
                    else if (sortCriteria.ByRating == "HIGHEST")
                    {
                        sortedList = unsortedList.OrderByDescending(r => r.Timestamp).ThenByDescending(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                    }
                    else if (sortCriteria.ByRating == "LOWEST")
                    {
                        sortedList = unsortedList.OrderByDescending(r => r.Timestamp).ThenBy(r => r.Comments.FirstOrDefault(c => c.UserId == r.CustomerId).Rating).ToList();
                    }
                }
            }
            return sortedList;
        }

        [HttpPost]
        [Route("api/rides/dispatcherRidesSearch")]
        public IHttpActionResult DispatcherRidesSearch(DispatcherRidesSearch searchParams)
        {
            List<Ride> searchedRides = new List<Ride>();
            if (String.IsNullOrWhiteSpace(searchParams.UserType))
            {
                searchedRides = _unitOfWork.RideRepository
                    .GetAllRidesIncludeAll().ToList();
            }
            else if (searchParams.UserType == "CUSTOMER")
            {
                if (String.IsNullOrWhiteSpace(searchParams.Name) && String.IsNullOrWhiteSpace(searchParams.Lastname))
                {
                    searchedRides = _unitOfWork.RideRepository
                        .GetAllRidesIncludeAll().ToList();
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(searchParams.Name) && String.IsNullOrWhiteSpace(searchParams.Lastname))
                    {
                        searchedRides = _unitOfWork.RideRepository
                            .FilterRidesIncludeAll(r => r.Customer.Name == searchParams.Name).ToList();
                    }
                    else if (String.IsNullOrWhiteSpace(searchParams.Name) && !String.IsNullOrWhiteSpace(searchParams.Lastname))
                    {
                        searchedRides = _unitOfWork.RideRepository
                            .FilterRidesIncludeAll(r => r.Customer.Lastname == searchParams.Lastname).ToList();
                    }
                    else
                    {
                        searchedRides = _unitOfWork.RideRepository
                            .FilterRidesIncludeAll(r => r.Customer.Name == searchParams.Name && 
                                                                                            r.Customer.Lastname == searchParams.Lastname).ToList();
                    }
                }
            }
            else if (searchParams.UserType == "DRIVER")
            {
                if (String.IsNullOrWhiteSpace(searchParams.Name) && String.IsNullOrWhiteSpace(searchParams.Lastname))
                {
                    searchedRides = _unitOfWork.RideRepository
                        .GetAllRidesIncludeAll().ToList();
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(searchParams.Name) && String.IsNullOrWhiteSpace(searchParams.Lastname))
                    {
                        searchedRides = _unitOfWork.RideRepository
                            .FilterRidesIncludeAll(r => r.Driver.Name == searchParams.Name).ToList();
                    }
                    else if (String.IsNullOrWhiteSpace(searchParams.Name) && !String.IsNullOrWhiteSpace(searchParams.Lastname))
                    {
                        searchedRides = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.Driver.Lastname == searchParams.Lastname).ToList();
                    }
                    else
                    {
                        searchedRides = _unitOfWork.RideRepository
                            .FilterRidesIncludeAll(r => r.Driver.Name == searchParams.Name &&
                                                                                            r.Driver.Lastname == searchParams.Lastname).ToList();
                    }
                }
            }
            List<RideDto> searchedRidesDto = _iMapper.Map<List<Ride>, List<RideDto>>(searchedRides);
            return Ok(searchedRidesDto);
        }

        [HttpPost]
        [Route("api/rides/dispatcherFormRide")]
        public IHttpActionResult DispatcherFormRide(DispatcherFormRideRequestModel dispatcherFormRideRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                Ride newRide = new Ride();
                newRide.StartLocation = _iMapper.Map<LocationDto, Location>(dispatcherFormRideRequest.Location);
                newRide.Timestamp = DateTime.Now;
                newRide.RideStatus = (int)RideStatus.FORMED;
                newRide.CarType = (int) CarType.PASSENGER;
                newRide.DriverId = dispatcherFormRideRequest.DriverId;
                newRide.DispatcherId = dispatcherFormRideRequest.DispatcherId;
                _unitOfWork.RideRepository.Add(newRide);
                _unitOfWork.Complete();

                RideDto newRideDto = _iMapper.Map<Ride, RideDto>(newRide);
                return CreatedAtRoute("DefaultApi", new { controller = "rides", id = newRideDto.Id }, newRideDto);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("api/rides/dispatcherProcessRide")]
        public IHttpActionResult DispatcherProcessRide(ProcessRideRequestModel rideProcessRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                Ride rideToProcess = _unitOfWork.RideRepository.FilterRidesIncludeAll(r => r.Id == rideProcessRequest.RideId).FirstOrDefault();
                rideToProcess.RideStatus = (int)RideStatus.PROCESSED;
                rideToProcess.DriverId = rideProcessRequest.DriverId;
                rideToProcess.DispatcherId = rideProcessRequest.DispatcherId;
                _unitOfWork.RideRepository.Update(rideToProcess);
                _unitOfWork.Complete();

                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest();
        }
    }
}