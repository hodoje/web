using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public UserRepository(DbContext context) : base(context) { }

        public IEnumerable<User> GetAllIncludeAll()
        {
            return _entities.Include(u => u.Car).Include(u => u.DriverLocation);
        }

        public User GetByIdIncludeAll(int id)
        {
            return _entities.Where(u => u.Id == id).Include(u => u.Car).Include(u => u.DriverLocation).SingleOrDefault();
        }
    }
}
