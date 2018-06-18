using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public abstract class User
    {
        public User()
        {
            CustomerRides = new HashSet<Ride>();
            DispatcherRides = new HashSet<Ride>();
            DriverRides = new List<Ride>();
            Comments = new HashSet<Comment>();
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
        [Required]
        public int Gender { get; set; }
        //[Required]
        public string NationalIdentificationNumber { get; set; }
        //[Required]
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
    }
}
