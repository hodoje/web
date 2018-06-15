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
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        public DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public UserRepository(DatabaseContext context) : base(context) { }
    }
}
