using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositoryInterfaces
{
    public interface IRideRepository : IRepository<Ride, int>
    {
        IEnumerable<Ride> GetAllUserRidesIncludeLocationAndComments(int userId);
        Ride GetRideByIdIncludeLocationAndComments(int id);
        IEnumerable<Ride> FilterUserRidesIncludeLocationAndComments(Expression<Func<Ride, bool>> predicate);
    }
}
