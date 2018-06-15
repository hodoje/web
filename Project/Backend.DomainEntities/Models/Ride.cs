using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
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
        public DateTime Timestamp { get; set; }
        public int StartLocationId { get; set; }
        [ForeignKey("StartLocationId")]
        public Location StartLocation { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        public CarType CarType { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int DispatcherId { get; set; }
        [ForeignKey("DispatcherId")]
        public Dispatcher Dispatcher { get; set; }
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        public double Price { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        public int RideStatusId { get; set; }
        [ForeignKey("RideStatusId")]
        public RideStatus RideStatus { get; set; }
    }
}
