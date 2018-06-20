using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.DataAccess.UnitOfWork;
using Backend.LoginRepository;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/access/{action}")]
    public class AccessController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoginRepository _loginRepository;

        public AccessController(IUnitOfWork unitOfWork, ILoginRepository loginRepository)
        {
            _unitOfWork = unitOfWork;
            _loginRepository = loginRepository;
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginModel userLogin)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Username == userLogin.Username) != null)
            {
                if (_loginRepository.Login(userLogin))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("User with this username does not exist.");
            }
        }

        [HttpPost]
        public IHttpActionResult Logout([FromBody] LoginModel userLogin)
        {
            if (_loginRepository.Logout(userLogin))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
