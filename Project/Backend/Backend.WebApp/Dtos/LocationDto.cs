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
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<User> Drivers { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStarts { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}