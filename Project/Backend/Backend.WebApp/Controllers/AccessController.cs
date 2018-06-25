﻿using System;
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

        public AccessController(IUnitOfWork unitOfWork, IAccessService accessService)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody]ApiMessage<string, LoginModel> user)
        {
            ApiMessage<string, LoginModel> allDataResponse;
            if ((allDataResponse = _accessService.Login(user, _unitOfWork)) != null)       // Need to inject unit of work because otherwise it would get disposed
            {
                // With this new message we get rid of the reference that allDataResponse has on MemoryCache
                ApiMessage<string, LoginModel> response = new ApiMessage<string, LoginModel>
                {
                    Key = allDataResponse.Key,
                    Data = new LoginModel
                    {
                        Username = null,
                        Password = null,
                        Role = allDataResponse.Data.Role
                    }
                };
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult Logout([FromBody]ApiMessage<string, LoginModel> user)
        {
            if (_accessService.Logout(user.Key))
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
