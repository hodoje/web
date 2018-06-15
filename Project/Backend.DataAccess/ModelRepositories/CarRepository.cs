using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class CarRepository : Repository<Car, int>, ICarRepository
    {
        public DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public CarRepository(DatabaseContext context) : base(context) { }
    }
}
