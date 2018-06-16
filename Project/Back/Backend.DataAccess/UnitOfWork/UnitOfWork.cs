using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositories;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Backend.DataAccess.TypeModelRepositories;
using Backend.DataAccess.TypeModelRepositoryInterface;

namespace Backend.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork() { }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        // Model repositories
        public virtual IAddressRepository AddressRepository { get; private set; }
        public virtual ICarRepository CarRepository { get; private set; }
        public virtual ICommentRepository CommentRepository { get; private set; }
        public virtual ILocationRepository LocationRepository { get; private set; }
        public virtual IRideRepository RideRepository { get; private set; }
        public virtual IUserRepository UserRepository { get; private set; }

        // TypeModel repositories
        public virtual ICarTypeRepository CarTypeRepository { get; private set; }
        public virtual IGenderRepository GenderRepository { get; private set; }
        public virtual IRideStatusRepository RideStatusRepository { get; private set; }
        public virtual IRoleRepository RoleRepository { get; private set; }

        public virtual int Complete()
        {
            return _context.SaveChanges();
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}
