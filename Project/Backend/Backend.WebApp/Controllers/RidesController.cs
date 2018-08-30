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
        [Route("api/rides/getAllMyRides")]
        [ResponseType(typeof(IEnumerable<RideDto>))]
        public IHttpActionResult GetAllMyRides()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                User user = _unitOfWork.UserRepository.Find(u => u.Username == loginModel.Username).FirstOrDefault();
                List<Ride> allUserRides = _unitOfWork.RideRepository.GetAllUserRidesIncludeLocationAndComment(user.Id).ToList();
                foreach (Ride r in allUserRides)
                {
                    if (r.Comment == null)
                    {
                        r.Comment = new Comment();
                    }
                }
                List<RideDto> allUserRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allUserRides);
                return Ok(allUserRidesDtos);
            }
            //if (_accessService.GetLoginData(hash, _unitOfWork).Data != null)
            //{
            //    LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
            //    User user = _unitOfWork.UserRepository.Find(u => u.Username == loginModel.Username).FirstOrDefault();
            //    List<Ride> allUserRides = _unitOfWork.RideRepository.GetAllUserRidesIncludeLocationAndComment(user.Id).ToList();
            //    foreach (Ride r in allUserRides)
            //    {
            //        if (r.Comment == null)
            //        {
            //            r.Comment = new Comment();
            //        }
            //    }
            //    List<RideDto> allUserRidesDtos = _iMapper.Map<List<Ride>, List<RideDto>>(allUserRides);
            //    return Ok(allUserRidesDtos);
            //}
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

            //Comment comment = new Comment();
            //comment.Id = newRide.Id;
            //_unitOfWork.CommentRepository.Add(comment);
            //_unitOfWork.Complete();

            //newRide.CommentId = comment.Id;
            //_unitOfWork.RideRepository.Update(newRide);
            //_unitOfWork.Complete();

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
            Ride oldRide = _unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
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
            Ride oldRide = _unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CREATED).FirstOrDefault();
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

            List<Ride> allUserRides = _unitOfWork.RideRepository.GetAllUserRidesIncludeLocationAndComment(user.Id).ToList();
            allUserRides = allUserRides.OrderByDescending(r => r.Timestamp).ToList(); //newest first
            Ride latestRide = allUserRides.FirstOrDefault(r => r.CustomerId == user.Id && r.RideStatus == (int)RideStatus.CANCELLED);
            if (latestRide != null)
            {
                // This check here is if something trips and comment for a latest cancelled ride is added multiple times
                Comment comment;
                if ((comment = _unitOfWork.CommentRepository.GetById(commentDto.Id)) == null)
                {
                    
                    comment = _iMapper.Map<CommentDto, Comment>(commentDto);
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : comment.Description;
                    comment.Id = latestRide.Id;
                    comment.UserId = user.Id;
                    comment.Timestamp = DateTime.Now;
                    _unitOfWork.CommentRepository.Add(comment);
                    _unitOfWork.Complete();
                }
                else
                {
                    comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : commentDto.Description;
                    comment.Timestamp = DateTime.Now;
                    _unitOfWork.CommentRepository.Update(comment);
                    _unitOfWork.Complete();
                }

                latestRide.CommentId = comment.Id;
                _unitOfWork.RideRepository.Update(latestRide);
                _unitOfWork.Complete();
            }


            //Comment comment = _iMapper.Map<CommentDto, Comment>(commentDto);
            //comment.Id = latestRide.Id;
            //comment.UserId = user.Id;
            //_unitOfWork.CommentRepository.Add(comment);
            //_unitOfWork.Complete();

            //latestRide.CommentId = comment.Id;
            //_unitOfWork.RideRepository.Update(latestRide);
            //_unitOfWork.Complete();

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

            Ride ride = _unitOfWork.RideRepository.GetRideByIdIncludeLocationAndComment(commentDto.Id);
            Comment comment;
            if ((comment = _unitOfWork.CommentRepository.GetById(commentDto.Id)) == null)
            {
                comment = _iMapper.Map<CommentDto, Comment>(commentDto);
                comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : comment.Description;
                comment.UserId = user.Id;
                comment.Timestamp = DateTime.Now;
                _unitOfWork.CommentRepository.Add(comment);
                _unitOfWork.Complete();
            }
            else
            {
                comment.Description = (String.IsNullOrWhiteSpace(comment.Description)) ? "" : commentDto.Description;
                comment.Timestamp = DateTime.Now;
                _unitOfWork.CommentRepository.Update(comment);
                _unitOfWork.Complete();
            }

            ride.CommentId = comment.Id;
            _unitOfWork.RideRepository.Update(ride);
            _unitOfWork.Complete();

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
            return Ok(comment.Rating);
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
                            (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByDate.From == null || refine.Search.ByDate.From == DateTime.MinValue)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Timestamp <= refine.Search.ByDate.To);
                        }
                        else if (refine.Search.ByDate.To == null || refine.Search.ByDate.To == DateTime.MinValue)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Timestamp >= refine.Search.ByDate.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
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
                            (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByRating.From == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comment.Rating <= refine.Search.ByRating.To);
                        }
                        else if (refine.Search.ByRating.To == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comment.Rating >= refine.Search.ByRating.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Comment.Rating >= refine.Search.ByRating.From &&
                                                                                                                     r.Comment.Rating <= refine.Search.ByRating.To);
                        }
                    }
                }

                if (shouldPriceSearch)
                {
                    // BY PRICE
                    if (refine.Search.ByPrice.From == null && refine.Search.ByPrice.To == null)
                    {
                        distinctList =
                            (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id);
                    }
                    else
                    {
                        if (refine.Search.ByPrice.From == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price <= refine.Search.ByPrice.To);
                        }
                        else if (refine.Search.ByRating.To == null)
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price >= refine.Search.ByPrice.From);
                        }
                        else
                        {
                            distinctList =
                                (List<Ride>)_unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id &&
                                                                                                                     r.Price >= refine.Search.ByPrice.From &&
                                                                                                                     r.Price <= refine.Search.ByPrice.To);
                        }
                    }
                }

                if (!shouldDateSearch && !shouldRatingSearch && !shouldRatingSearch)
                {
                    distinctList = _unitOfWork.RideRepository.GetAllUserRidesIncludeLocationAndComment(user.Id).ToList();
                }

                distinctList = distinctList.GroupBy(r => r.Id).Select(group => group.FirstOrDefault()).ToList();

                refinedRides = SortList(refine.Sort, distinctList);
            }
            else
            {
                // FILTER
                List<Ride> filteredList = _unitOfWork.RideRepository.FilterUserRidesIncludeLocationAndComment(r => r.CustomerId == user.Id && 
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
                    sortedList = unsortedList.OrderByDescending(r => r.Comment.Rating).ToList();
                }
                else if (sortCriteria.ByRating == "LOWEST")
                {
                    sortedList = unsortedList.OrderBy(r => r.Comment.Rating).ToList();
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
                        sortedList = unsortedList.OrderBy(r => r.Timestamp).ThenByDescending(r => r.Comment.Rating).ToList();
                    }
                    else if (sortCriteria.ByRating == "LOWEST")
                    {
                        sortedList = unsortedList.OrderBy(r => r.Timestamp).ThenBy(r => r.Comment.Rating).ToList();
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
                        sortedList = unsortedList.OrderByDescending(r => r.Timestamp).ThenByDescending(r => r.Comment.Rating).ToList();
                    }
                    else if (sortCriteria.ByRating == "LOWEST")
                    {
                        sortedList = unsortedList.OrderByDescending(r => r.Timestamp).ThenBy(r => r.Comment.Rating).ToList();
                    }
                }
            }
            return sortedList;
        }
    }
}