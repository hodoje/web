﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        public int RideId { get; set; }
        [ForeignKey("RideId")]
        public Ride Ride { get; set; }
        [Range(0, 5)]
        [Required]
        public int Rating { get; set; }
    }
}
