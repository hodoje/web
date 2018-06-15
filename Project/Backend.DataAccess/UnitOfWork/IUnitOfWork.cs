using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Backend.DataAccess.TypeModelRepositoryInterface;

namespace Backend.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Model repositories
        IAddressRepository AddressRepository { get; }
        ICarRepository CarRepository { get; }
        ICommentRepository CommentRepository { get; }
        ILocationRepository LocationRepository { get; }
        IRideRepository RideRepository { get; }
        IUserRepository UserRepository { get; }

        // TypeModel repositories
        ICarTypeRepository CarTypeRepository { get; }
        IGenderRepository GenderRepository { get; }
        IRideStatusRepository RideStatusRepository { get; }
        IRoleRepository RoleRepository { get; }
        int Complete();
    }
}
