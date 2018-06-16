using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.TypeModelRepositoryInterface;
using DomainEntities;
using DomainEntities.EnumDatabaseModels;

namespace Backend.DataAccess.TypeModelRepositories
{
    public class GenderRepository : Repository<Gender, int>, IGenderRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public GenderRepository(DbContext context) : base(context) { }
    }
}
