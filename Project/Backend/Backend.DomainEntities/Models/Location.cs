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
            Address = new Address();
        }

        [Key]
        public int Id { get; set; }
        public Address Address { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        [InverseProperty("DriverLocation")]
        public virtual ICollection<User> Drivers { get; set; }
        [InverseProperty("StartLocation")]
        public virtual ICollection<Ride> RideStarts { get; set; }
        [InverseProperty("DestinationLocation")]
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}
