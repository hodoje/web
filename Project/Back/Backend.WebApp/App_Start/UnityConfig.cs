using System.Data.Entity;
using System.Web.Http;
using Backend.DataAccess;
using Backend.DataAccess.ModelRepositories;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Backend.DataAccess.TypeModelRepositories;
using Backend.DataAccess.TypeModelRepositoryInterface;
using Backend.DataAccess.UnitOfWork;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace Backend
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<DbContext, DatabaseContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<ICarRepository, CarRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<ILocationRepository, LocationRepository>();
            container.RegisterType<IRideRepository, RideRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<ICarTypeRepository, CarTypeRepository>();
            container.RegisterType<IGenderRepository, GenderRepository>();
            container.RegisterType<IRideStatusRepository, RideStatusRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}