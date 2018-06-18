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
        public Location()
        {
            Drivers = new HashSet<Driver>();
            RideStarts = new HashSet<Ride>();
            RideDestinations = new HashSet<Ride>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public double CoordinateX { get; set; }
        [Required]
        public double CoordinateY { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<Driver> Drivers { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStarts { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}
