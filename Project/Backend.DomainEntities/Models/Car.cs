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
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        public int YearOfManufactoring { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxiNumber { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        public CarType CarType { get; set; }
    }
}
