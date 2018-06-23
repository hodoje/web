using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    [ComplexType]
    public class Car
    {
        public int? YearOfManufactoring { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxiNumber { get; set; }
        public int? CarType { get; set; }
    }
}
