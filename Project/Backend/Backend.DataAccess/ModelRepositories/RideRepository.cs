using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class RideRepository : Repository<Ride, int>, IRideRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public RideRepository(DbContext context) : base(context) { }
        public IEnumerable<Ride> GetAllUserRidesIncludeLocationAndComments(int userId)
        {
            return _entities.Where(r => r.CustomerId == userId).Include(r => r.StartLocation).Include(r => r.Comments);
        }

        public Ride GetRideByIdIncludeLocationAndComments(int id)
        {
            return _entities.Where(r => r.Id == id).Include(r => r.StartLocation).Include(r => r.Comments).FirstOrDefault();
        }

        public IEnumerable<Ride> FilterUserRidesIncludeLocationAndComments(Expression<Func<Ride, bool>> predicate)
        {
            List<Ride> filteredRides = new List<Ride>();
            filteredRides = _entities.Where(predicate).Include(r => r.StartLocation).Include(r => r.Comments).ToList();
            return filteredRides;
        }
    }
}
