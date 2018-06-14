using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.Models;

namespace Backend.Controllers
{
    public class DefaultController : ApiController
    {
        public Default GetDefault()
        {
            return new Default {DefaultName = "Nikola", DefaultLastName = "Karaklic"};
        }
    }
}
