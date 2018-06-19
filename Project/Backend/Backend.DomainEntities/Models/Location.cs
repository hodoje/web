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
            Drivers = new HashSet<User>();
            RideStarts = new HashSet<Ride>();
            RideDestinations = new HashSet<Ride>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public double CoordinateX { get; set; }
        [Required]
        public double CoordinateY { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<User> Drivers { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStarts { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}
