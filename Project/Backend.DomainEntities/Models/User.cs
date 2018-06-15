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
    public abstract class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public virtual string GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }
        [Required]
        public string NationalIdentificationNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public virtual string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public virtual ICollection<Ride> Rides { get; set; }
    }
}
