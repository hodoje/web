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
        public UserDto GetUsers()
        {
            Driver u = (Driver)_unitOfWork.UserRepository.GetById(1);
            u.Car = _unitOfWork.CarRepository.GetById(u.CarId);
            u.DriverLocation = _unitOfWork.LocationRepository.GetById(u.DriverLocationId);
            UserDto ud = _iMapper.Map<Driver, UserDto>(u);
            return ud;
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

            Driver d = _iMapper.Map<UserDto, Driver>(ud);
            Customer c = _iMapper.Map<UserDto, Customer>(ud);
            Dispatcher dp = _iMapper.Map<UserDto, Dispatcher>(ud);

            DriverDto ddto = _iMapper.Map<Driver, DriverDto>(d);
            CustomerDto cdto = _iMapper.Map<Customer, CustomerDto>(c);
            DispatcherDto dpdto = _iMapper.Map<Dispatcher, DispatcherDto>(dp);

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

        //public Driver GetDriverByIdIncludeAll(int id)
        //{
        //    Driver d = (Driver)_unitOfWork.UserRepository.GetById(1);
        //    d.Car = _unitOfWork.CarRepository.GetById(d.CarId);
        //    d.DriverLocation = _unitOfWork.LocationRepository.GetById(d.DriverLocationId);
        //    return d;
        //}
    }
}