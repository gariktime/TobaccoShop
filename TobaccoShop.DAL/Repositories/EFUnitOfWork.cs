using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Identity;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        //классы Identity
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        //репозитории продуктов
        private ProductGRepository<Product> productRepository;
        private ProductGRepository<Hookah> hookahRepository;

        //репозиторий заказов
        private OrderRepository orderRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }

        #region Работа с Identity

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        #endregion

        #region Репозитории продуктов

        public IGenericRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductGRepository<Product>(db);
                return productRepository;
            }
        }

        public IGenericRepository<Hookah> Hookahs
        {
            get
            {
                if (hookahRepository == null)
                    hookahRepository = new ProductGRepository<Hookah>(db);
                return hookahRepository;
            }
        }

        #endregion

        public OrderRepository Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
