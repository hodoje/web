using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.EnumDatabaseModels;
using DomainEntities.Models;

namespace Backend.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<CarType> CarTypes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<RideStatus> RideStatuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
    }
}
