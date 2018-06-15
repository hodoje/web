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
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public virtual string GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public virtual string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public virtual ICollection<Ride> Rides { get; set; }
    }
}
