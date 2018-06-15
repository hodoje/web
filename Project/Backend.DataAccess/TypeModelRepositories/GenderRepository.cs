using System;
using System.Collections.Generic;
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
        public DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public GenderRepository(DatabaseContext context) : base(context) { }
    }
}
