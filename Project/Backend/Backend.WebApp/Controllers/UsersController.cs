using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Backend.DataAccess;
using Backend.DataAccess.UnitOfWork;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _iMapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
        }

        // GET: api/Users
        //public UserDto GetUsers()
        public IEnumerable<UserDto> GetUsers()
        {
            //Driver u = GetDriverByIdIncludeAll(1);
            //UserDto ud = _iMapper.Map<Driver, UserDto>(u);
            //IEnumerable<Driver> drivers = GetAllDriversIncludeAll();
            //IEnumerable<UserDto> userDtos = _iMapper.Map<IEnumerable<Driver>, IEnumerable<UserDto>> (drivers);
            //return userDtos;
            return new List<UserDto>();
        }

        //// GET: api/Users/5
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> GetUser(int id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        //// PUT: api/Users/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutUser(int id, User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Users
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult PostUser(UserDto user)
        {
            UserDto ud = user;

            User u = _iMapper.Map<UserDto, User>(ud);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //_unitOfWork.UserRepository.Add(user);
            _unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        //// DELETE: api/Users/5
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> DeleteUser(int id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();

        //    return Ok(user);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool UserExists(int id)
        //{
        //    return db.Users.Count(e => e.Id == id) > 0;
        //}

        //private User GetDriverByIdIncludeAll(int id)
        //{
        //    User u = _unitOfWork.UserRepository.GetById(1);
        //    d.Car = _unitOfWork.CarRepository.GetById(d.CarId);
        //    d.DriverLocation = _unitOfWork.LocationRepository.GetById(d.DriverLocationId);
        //    d.DriverLocation.Address = _unitOfWork.AddressRepository.GetById(d.DriverLocation.AddressId);
        //    return d;
        //}

        //private IEnumerable<Driver> GetAllDriversIncludeAll()
        //{
        //    IEnumerable<Driver> drivers = (IEnumerable<Driver>)_unitOfWork.UserRepository.GetAll();
        //    foreach (Driver d in drivers)
        //    {
        //        d.Car = _unitOfWork.CarRepository.GetById(d.CarId);
        //        d.DriverLocation = _unitOfWork.LocationRepository.GetById(d.DriverLocationId);
        //        d.DriverLocation.Address = _unitOfWork.AddressRepository.GetById(d.DriverLocation.AddressId);
        //    }
        //    return drivers;
        //}
    }
}