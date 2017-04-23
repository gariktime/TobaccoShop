using Ninject.Modules;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;
using TobaccoShop.DAL.Repositories;

namespace TobaccoShop.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;

        public ServiceModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IGenericRepository<Product>>().To<ProductGRepository<Product>>();
            Bind<IGenericRepository<Hookah>>().To<ProductGRepository<Hookah>>();
        }
    }
}
