using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Dtos
{
    public class AddressDto
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
    }
}