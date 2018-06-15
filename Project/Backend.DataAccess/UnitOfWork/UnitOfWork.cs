using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly DatabaseContext _context;

        public UnitOfWork() { }

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

            AddressRepository = new AddressRepository(_context);
            CarRepository = new CarRepository(_context);
            CommentRepository = new CommentRepository(_context);
            LocationRepository = new LocationRepository(_context);
            RideRepository = new RideRepository(_context);
            UserRepository = new UserRepository(_context);

            CarTypeRepository = new CarTypeRepository(_context);
            GenderRepository = new GenderRepository(_context);
            RideStatusRepository = new RideStatusRepository(_context);
            RoleRepository = new RoleRepository(_context);

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
