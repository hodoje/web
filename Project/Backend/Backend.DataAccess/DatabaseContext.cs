using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Models;

namespace Backend.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
    }
}
