//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using AutoMapper;
//using Backend.DataAccess;
//using Backend.DataAccess.UnitOfWork;
//using Backend.Dtos;
//using DomainEntities.Models;

//namespace Backend.Controllers
//{
//    public class LocationsController : ApiController
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private IMapper _iMapper;

//        public LocationsController(IUnitOfWork unitOfWork, IMapper iMapper)
//        {
//            _unitOfWork = unitOfWork;
//            _iMapper = iMapper;
//        }

//        // GET: api/Locations
//        [HttpGet]
//        [ResponseType(typeof(IEnumerable<LocationDto>))]
//        public IHttpActionResult GetLocations()
//        {
//            IEnumerable<Location> locations = _unitOfWork.LocationRepository.GetAll();
//            if (locations == null || locations.Count() < 1)
//            {
//                return NotFound();
//            }
//            IEnumerable<LocationDto> locatinoDtos = _iMapper.Map<IEnumerable<Location>, IEnumerable<LocationDto>>(locations);
//            return Ok(locatinoDtos);
//        }

//        // GET: api/Locations/5
//        [HttpGet]
//        [ResponseType(typeof(LocationDto))]
//        public IHttpActionResult GetLocation(int id)
//        {
//            Location location = _unitOfWork.LocationRepository.GetById(id);
//            if (location == null)
//            {
//                return NotFound();
//            }
//            LocationDto locationDto = _iMapper.Map<Location, LocationDto>(location);
//            return Ok(locationDto);
//        }

//        // PUT: api/Locations/5
//        [HttpPut]
//        [ResponseType(typeof(void))]
//        public IHttpActionResult PutLocation(int id, LocationDto locationDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != locationDto.Id)
//            {
//                return BadRequest();
//            }

//            Location location = _iMapper.Map<LocationDto, Location>(locationDto);

//            try
//            {
//                _unitOfWork.LocationRepository.Add(location);
//                _unitOfWork.Complete();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!LocationExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/Locations
//        [ResponseType(typeof(LocationDto))]
//        public IHttpActionResult PostLocation(LocationDto locationDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            Location location = _iMapper.Map<LocationDto, Location>(locationDto);

//            try
//            {
//                _unitOfWork.LocationRepository.Add(location);
//                _unitOfWork.Complete();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//            }

//            return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
//        }

//        // DELETE: api/Locations/5
//        [ResponseType(typeof(Location))]
//        public IHttpActionResult DeleteLocation(int id)
//        {
//            Location location = db.Locations.Find(id);
//            if (location == null)
//            {
//                return NotFound();
//            }

//            db.Locations.Remove(location);
//            db.SaveChanges();

//            return Ok(location);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool LocationExists(int id)
//        {
//            return db.Locations.Count(e => e.Id == id) > 0;
//        }
//    }
//}