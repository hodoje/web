using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Models;

namespace DomainEntities.EnumDatabaseModels
{
    public class RideStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StatusName { get; set; }
        public virtual ICollection<Ride> Rides { get; set; }
    }
}
