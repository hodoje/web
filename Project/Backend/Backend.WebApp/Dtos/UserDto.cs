using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class UserDto
    {
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
        public string Gender { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool IsBanned { get; set; }
        [Required]
        public string Role { get; set; }
        public List<Ride> CustomerRides { get; set; }
        public List<Ride> DispatcherRides { get; set; }
        public List<Ride> DriverRides { get; set; }
        public List<Comment> Comments { get; set; }
        public LocationDto DriverLocation { get; set; }
        public CarDto Car { get; set; }
    }
}