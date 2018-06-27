using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public virtual ICollection<User> Drivers { get; set; }
        public virtual ICollection<Ride> RideStarts { get; set; }
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}