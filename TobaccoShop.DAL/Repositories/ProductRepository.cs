using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationContext db;

        public ProductRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Delete(Guid productId)
        {
            Product p = db.Products.Find(productId);
            if (p != null)
                db.Products.Remove(p);
        }

        public Product FindById(Guid productId)
        {
            return db.Products.Include("Comments.User").FirstOrDefault(p => p.ProductId == productId);
        }

        public async Task<Product> FindByIdAsync(Guid productId)
        {
            return await db.Products.Include("Comments.User").FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public List<Product> GetAll()
        {
            return db.Products.AsNoTracking().ToList();
        }

        public List<Product> GetAll(Func<Product, bool> predicate)
        {
            return db.Products.AsNoTracking().Where(predicate).ToList();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate)
        {
            return await db.Products.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
