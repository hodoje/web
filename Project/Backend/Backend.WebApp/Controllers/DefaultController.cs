using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.DataAccess;
using Backend.DataAccess.TypeModelRepositories;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;
using DomainEntities.EnumDatabaseModels;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class DefaultController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DefaultController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Default GetDefault()
        {
            return new Default {DefaultName = "Nikola", DefaultLastName = "Karaklic"};
        }
    }
}
