using System;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        System.Data.Entity.Database Database { get; }

        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IUserRepository Users { get; }

        IProductRepository Products { get; }

        IGenericRepository<Hookah> Hookahs { get; }
        IGenericRepository<HookahTobacco> HookahTobacco { get; }

        IOrderRepository Orders { get; }
        ICommentRepository Comments { get; }

        void Save();
        Task SaveAsync();
    }
}
