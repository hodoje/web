using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int RideId { get; set; }
        [ForeignKey("RideId")]
        public Ride Ride { get; set; }
        [Range(0, 5)]
        public int Rating { get; set; }
    }
}
