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
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        public DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public CommentRepository(DatabaseContext context) : base(context) { }
    }
}
