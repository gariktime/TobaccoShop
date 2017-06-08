using System;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Identity;
using TobaccoShop.DAL.Repositories;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        System.Data.Entity.Database Database { get; }

        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IGenericRepository<Product> Products { get; }
        IGenericRepository<Hookah> Hookahs { get; }

        OrderRepository Orders { get; }

        void Save();
        Task SaveAsync();
    }
}
