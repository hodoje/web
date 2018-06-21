using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    // For simplicity we are using only one class model for our users that has all possible properties listed in assignment text
    public class User
    {
        public User()
        {
            CustomerRides = new HashSet<Ride>();
            DispatcherRides = new HashSet<Ride>();
            DriverRides = new List<Ride>();
            Comments = new HashSet<Comment>();
            Car = new Car();                        // Must be instantiated so EF can work it out, otherwise we'll get a run-time exception
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lastname { get; set; }
        public int Gender { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool IsBanned { get; set; }
        [Required]
        public int Role { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Ride> CustomerRides { get; set; }
        [InverseProperty("Dispatcher")]
        public virtual ICollection<Ride> DispatcherRides { get; set; }
        [InverseProperty("Driver")]
        public virtual ICollection<Ride> DriverRides { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public int? DriverLocationId { get; set; }
        [ForeignKey("DriverLocationId")]
        public Location DriverLocation { get; set; }
        public Car Car { get; set; }
    }
}
