using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Car
    {
        [Key, ForeignKey("Driver")]
        public int Id { get; set; }
        public User Driver { get; set; }
        [Required]
        public int YearOfManufactoring { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string TaxiNumber { get; set; }
        [Required]
        public int CarType { get; set; }
    }
}
