using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public override string ToString()
        {
            return $"{StreetName} {StreetNumber}, {City}, {PostalCode}";
        }
    }
}
