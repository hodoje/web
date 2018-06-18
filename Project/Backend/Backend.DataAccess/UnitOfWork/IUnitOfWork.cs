﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;

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

        int Complete();
    }
}
