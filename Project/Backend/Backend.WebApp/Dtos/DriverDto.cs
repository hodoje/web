using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Dtos
{
    public class DriverDto : UserDto
    {
        public LocationDto DriverLocation { get; set; }
        public CarDto Car { get; set; }
    }
}