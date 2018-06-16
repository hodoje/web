﻿using System;
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
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public CommentRepository(DbContext context) : base(context) { }
    }
}
