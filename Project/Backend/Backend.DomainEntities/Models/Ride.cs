using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int StartLocationId { get; set; }
        [ForeignKey("StartLocationId")]        
        public Location StartLocation { get; set; }
        [Required]
        [DefaultValue(1)]
        public int CarType { get; set; }        
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
        [Required]
        public int DestinationLocationId { get; set; }
        [ForeignKey("DestinationLocationId")]        
        public Location DestinationLocation { get; set; }
        public int? DispatcherId { get; set; }
        [ForeignKey("DispatcherId")]        
        public User Dispatcher { get; set; }
        public int? DriverId { get; set; }
        [ForeignKey("DriverId")]
        public User Driver { get; set; }
        public double Price { get; set; }
        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        [Required]
        public int RideStatus { get; set; }
    }
}
