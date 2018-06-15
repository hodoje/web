using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.EnumDatabaseModels;

namespace DomainEntities.Models
{
    public class Ride
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        //public int StartLocationId { get; set; }
        [ForeignKey("LocationId")]
        public StartLocation StartLocation { get; set; }
        [Required]
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        public CarType CarType { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [Required]
        //public int DestinationId { get; set; }
        [ForeignKey("LocationId")]        
        public DestinationLocation Destination { get; set; }
        public int DispatcherId { get; set; }
        [ForeignKey("DispatcherId")]        
        public Dispatcher Dispatcher { get; set; }
        [Required]
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        public double Price { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        [Required]
        public int RideStatusId { get; set; }
        [ForeignKey("RideStatusId")]
        public RideStatus RideStatus { get; set; }
    }
}
