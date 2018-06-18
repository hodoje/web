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
        public RideStatus()
        {
            Rides = new HashSet<Ride>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Ride> Rides { get; set; }
    }
}
