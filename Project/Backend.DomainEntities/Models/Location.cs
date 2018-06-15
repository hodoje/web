using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public abstract class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double CoordinateX { get; set; }
        [Required]
        public double CoordinateY { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
    }
}
