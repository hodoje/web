using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Driver : User
    {
        [Required]
        public int DriverLocationId { get; set; }
        [ForeignKey("DriverLocationId")]
        public Location DriverLocation { get; set; }
        [Required]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }
    }
}
