using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Backend.DataAccess.TypeModelRepositoryInterface;
using Unity.Attributes;

namespace Backend.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Model repositories
        [Dependency]
        IAddressRepository AddressRepository { get; }
        [Dependency]
        ICarRepository CarRepository { get; }
        [Dependency]
        ICommentRepository CommentRepository { get; }
        [Dependency]
        ILocationRepository LocationRepository { get; }
        [Dependency]
        IRideRepository RideRepository { get; }
        [Dependency]
        IUserRepository UserRepository { get; }

        // TypeModel repositories
        [Dependency]
        ICarTypeRepository CarTypeRepository { get; }
        [Dependency]
        IGenderRepository GenderRepository { get; }
        [Dependency]
        IRideStatusRepository RideStatusRepository { get; }
        [Dependency]
        IRoleRepository RoleRepository { get; }
        int Complete();
    }
}
