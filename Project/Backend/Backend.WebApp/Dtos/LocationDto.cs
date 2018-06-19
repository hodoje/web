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
        [Key]
        public int Id { get; set; }
        [Required]
        public double CoordinateX { get; set; }
        [Required]
        public double CoordinateY { get; set; }
        [Required]
        public AddressDto Address { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<Driver> Drivers { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStarts { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}