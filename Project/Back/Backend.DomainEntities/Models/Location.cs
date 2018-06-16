using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double CoordinateX { get; set; }
        [Required]
        public double CoordinateY { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<Driver> DriverLocations { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStart { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestination { get; set; }
    }
}
