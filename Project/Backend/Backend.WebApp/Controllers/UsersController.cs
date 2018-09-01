using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AutoMapper;
using Backend.AccessServices;
using Backend.DataAccess;
using Backend.DataAccess.UnitOfWork;
using Backend.Dtos;
using Backend.Models;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _iMapper;
        private IAccessService _accessService;
        private HashGenerator _hashGenerator;

        public UsersController(IUnitOfWork unitOfWork, IMapper iMapper, IAccessService accessService, HashGenerator hashGenerator)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
            _accessService = accessService;
            _hashGenerator = hashGenerator;
        }

        // GET: api/Users
        [HttpGet]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        [Route("api/users/getAllUsers")]
        public IHttpActionResult GetUsers()
        {
            //LoginModel lm = new LoginModel {Username = "agsa", Password = "asg"};
            //if (_loginRepository.IsLoggedIn(lm))
            //{
            IEnumerable<User> users = _unitOfWork.UserRepository.GetAllIncludeAll();
            if (users == null || users.Count() < 1)
            {
                return NotFound();
            }
            IEnumerable<UserDto> userDtos = _iMapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return Ok(userDtos);
            //}
            //return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Forbidden));
        }

        // GET: api/Users/5
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser(int id)
        {
            User user = _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            UserDto userDto = _iMapper.Map<User, UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet]
        [Route("api/users/getuserbyusername")]
        public IHttpActionResult GetUser()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            if (_accessService.IsLoggedIn(hash))
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                User user = new User();
                if (loginModel.Role == "CUSTOMER" || loginModel.Role == "DISPATCHER")
                {
                    user = _unitOfWork.UserRepository.GetUserByUsername(loginModel.Username, loginModel.Role);
                }
                else if (loginModel.Role == "DRIVER")
                {
                    user = _unitOfWork.UserRepository.GetUserByUsernameIncludeAll(loginModel.Username, loginModel.Role);
                }
                UserDto userDto = _iMapper.Map<User, UserDto>(user);
                return Ok(userDto);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/users/getAllDrivers")]
        public IHttpActionResult GetAllDrivers()
        {
            string hash = _accessService.ExtractHash(Request.Headers.Authorization.Parameter);
            List<UserDto> driverDtoList = new List<UserDto>();
            ApiMessage<string, LoginModel> loginData = _accessService.GetLoginData(hash, _unitOfWork);
            if (loginData != null)
            {
                LoginModel loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
                if (loginModel.Role == "DISPATCHER")
                {
                    driverDtoList = _iMapper.Map<List<User>, List<UserDto>>(_unitOfWork.UserRepository.GetAllDriversIncludeLocationAndCar().ToList());
                }
            }
            return Ok(driverDtoList);
        }

        //PUT: api/Users/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, ApiMessage<string, UserDto> updatedUserApiMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedUserApiMessage.Data.Id)
            {
                return BadRequest();
            }

            string hash = updatedUserApiMessage.Key;

            try
            {
                User oldUser = _unitOfWork.UserRepository.GetById(id);
                string oldUsername = oldUser.Username;
                string oldPassword = oldUser.Password;
                User updatedUser = _iMapper.Map<UserDto, User>(updatedUserApiMessage.Data);

                foreach (PropertyInfo property in typeof(User).GetProperties())
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(oldUser, property.GetValue(updatedUser, null), null);
                    }
                }

                _unitOfWork.UserRepository.Update(oldUser);
                _unitOfWork.Complete();

                if (oldUsername != updatedUser.Username || oldPassword != updatedUser.Password)
                {
                    _accessService.Logout(updatedUserApiMessage.Key);
                    ApiMessage<string, LoginModel> newLogin = new ApiMessage<string, LoginModel>();
                    newLogin.Key = null;
                    newLogin.Data = new LoginModel
                    {
                        Username = updatedUser.Username,
                        Password = updatedUser.Password,
                        Role = ((Role)updatedUser.Role).ToString()
                    };
                    hash = _accessService.Login(newLogin, _unitOfWork).Key;
                }

            }
            catch (DbUpdateException)
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

            return Ok(hash);
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

            try
            {
                if (_unitOfWork.UserRepository.Find(u => u.Username == user.Username).FirstOrDefault() == null)
                {
                    _unitOfWork.UserRepository.Add(user);
                    _unitOfWork.Complete();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
            }
            _iMapper.Map<User, UserDto>(user, userDto);

            return CreatedAtRoute("DefaultApi", new { id = userDto.Id }, userDto);
        }

        //DELETE: api/Users/5
        [HttpDelete]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = _unitOfWork.UserRepository.GetById(id);
            Car usersCar = _unitOfWork.CarRepository.GetById(id);
            UserDto userDto = _iMapper.Map<User, UserDto>(user);
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.CarRepository.Remove(usersCar);
            _unitOfWork.Complete();
            
            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.Complete();

            return Ok(userDto);
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