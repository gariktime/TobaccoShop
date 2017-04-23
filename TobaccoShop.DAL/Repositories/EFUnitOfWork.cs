using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ProductContext db;
        private ProductGRepository<Product> productRepository;
        private ProductGRepository<Hookah> hookahRepository;
        private OrderRepository orderRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new ProductContext(connectionString);
        }

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

        //public ProductRepository Products
        //{
        //    get
        //    {
        //        if (productRepository == null)
        //            productRepository = new ProductRepository(db);
        //        return productRepository;
        //    }
        //}

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

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
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
