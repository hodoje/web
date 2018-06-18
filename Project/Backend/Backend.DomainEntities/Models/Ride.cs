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
        public virtual Location StartLocation { get; set; }
        [Required]
        [DefaultValue(1)]
        public int CarType { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [Required]
        public int DestinationLocationId { get; set; }
        [ForeignKey("DestinationLocationId")]        
        public virtual Location DestinationLocation { get; set; }
        public int DispatcherId { get; set; }
        [ForeignKey("DispatcherId")]        
        public virtual Dispatcher Dispatcher { get; set; }
        [Required]
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }
        public double Price { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
        [Required]
        public int RideStatus { get; set; }
    }
}
