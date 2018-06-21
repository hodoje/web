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
using Backend.LoginRepository;
using Backend.Models;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _iMapper;
        private ILoginRepository _loginRepository;

        public UsersController(IUnitOfWork unitOfWork, IMapper iMapper, ILoginRepository loginRepository)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
            _loginRepository = loginRepository;
        }

        // GET: api/Users
        [HttpGet]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {
            LoginModel lm = new LoginModel {Username = "agsa", Password = "asg"};
            if (_loginRepository.IsLoggedIn(lm))
            {
                IEnumerable<User> users = _unitOfWork.UserRepository.GetAllIncludeAll();
                if (users == null)
                {
                    return NotFound();
                }
                IEnumerable<UserDto> userDtos = _iMapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
                return Ok(userDtos);
            }
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Forbidden));
        }

        // GET: api/Users/5
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser(int id)
        {
            User user = _unitOfWork.UserRepository.GetByIdIncludeAll(id);
            if (user == null)
            {
                return NotFound();
            }
            UserDto userDto = _iMapper.Map<User, UserDto>(user);
            return Ok(userDto);
        }

        //PUT: api/Users/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDto.Id)
            {
                return BadRequest();
            }

            User user = _iMapper.Map<UserDto, User>(userDto);

            try
            {
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [HttpPost]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult PostUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _iMapper.Map<UserDto, User>(userDto);

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Complete();

            _iMapper.Map<User, UserDto>(user, userDto);

            return CreatedAtRoute("DefaultApi", new { id = userDto.Id }, userDto);
        }

        //DELETE: api/Users/5
        [HttpDelete]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = _unitOfWork.UserRepository.GetByIdIncludeAll(id);
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.Complete();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return _unitOfWork.UserRepository.GetById(id) != null;
        }
    }
}