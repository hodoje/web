using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Models;

namespace DomainEntities.EnumDatabaseModels
{
    public class CarType
    {
        public CarType()
        {
            Cars = new HashSet<Car>();
            Rides = new HashSet<Ride>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Ride> Rides { get; set; }
    }
}
