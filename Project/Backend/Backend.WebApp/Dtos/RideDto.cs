using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class RideDto
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public LocationDto StartLocation { get; set; }
        [Required]
        public string CarType { get; set; }
        [Required]
        public CustomerDto Customer { get; set; }
        [Required]
        public LocationDto DestinationLocation { get; set; }
        public DispatcherDto Dispatcher { get; set; }
        [Required]
        public DriverDto Driver { get; set; }
        public double Price { get; set; }
        public CommentDto Comment { get; set; }
        [Required]
        public string RideStatus { get; set; }
    }
}