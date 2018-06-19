using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class CommentDto
    {
        [Key, ForeignKey("Ride")]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public UserDto User { get; set; }
        [Required]
        public RideDto Ride { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}