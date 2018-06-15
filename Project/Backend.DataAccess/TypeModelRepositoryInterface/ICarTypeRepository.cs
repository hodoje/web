using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainEntities.EnumDatabaseModels;

namespace Backend.DataAccess.TypeModelRepositoryInterface
{
    public interface ICarTypeRepository : IRepository<CarType, int>
    {
    }
}
