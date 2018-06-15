using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.EnumDatabaseModels;

namespace DomainEntities.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        [Required]
        public int YearOfManufactoring { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string TaxiNumber { get; set; }
        [Required]
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        public CarType CarType { get; set; }
    }
}
