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
        private IUserRepository clientManager;

        //общий репозиторий продуктов
        private IProductRepository productRepository;

        //репозитории продуктов
        private IGenericRepository<Hookah> hookahRepository;
        private IGenericRepository<HookahTobacco> hookahTobaccoRepository;

        //репозиторий заказов
        private IOrderRepository orderRepository;

        //репозиторий комментариев к товарам
        private ICommentRepository commentRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new UserRepository(db);
        }

        public System.Data.Entity.Database Database
        {
            get
            {
                return db.Database;
            }
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

        public IUserRepository ClientManager
        {
            get { return clientManager; }
        }

        #endregion

        #region Репозитории продуктов

        public IProductRepository Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
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

        public IGenericRepository<HookahTobacco> HookahTobacco
        {
            get
            {
                if (hookahTobaccoRepository == null)
                    hookahTobaccoRepository = new ProductGRepository<HookahTobacco>(db);
                return hookahTobaccoRepository;
            }
        }

        #endregion

        public IOrderRepository Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(db);
                return commentRepository;
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
