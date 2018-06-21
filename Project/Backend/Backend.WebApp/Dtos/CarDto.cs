using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class CarDto
    {
        [Required]
        public int YearOfManufactoring { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string TaxiNumber { get; set; }
        [Required]
        public string CarType { get; set; }
    }
}