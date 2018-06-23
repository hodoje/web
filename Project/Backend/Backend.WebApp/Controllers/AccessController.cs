using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.AccessServices;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/access/{action}")]
    public class AccessController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _accessService;
        private readonly HashGenerator _hashGenerator;

        public AccessController(IUnitOfWork unitOfWork, IAccessService accessService, HashGenerator hashGenerator)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _hashGenerator = hashGenerator;
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginModel user)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Username == user.Username).Any())
            {
                ApiMessage<string, LoginModel> response;
                if ((response = _accessService.Login(user)) != null)
                {
                    return Ok(response);
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
        public IHttpActionResult Logout([FromBody]ApiMessage<string, LoginModel> user)
        {
            if (_accessService.Logout(user.Key))
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
