using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private ProductContext db;

        public ProductRepository(ProductContext context)
        {
            this.db = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

        public void Create(Product product)
        {
            if (product is Hookah)
                db.Hookahs.Add(product as Hookah);
            else if (product is HookahTobacco)
                db.HookahTobacco.Add(product as HookahTobacco);
            //db.Products.Add(product);
        }

        public void Update(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }

        public IEnumerable<Product> Find(Func<Product, Boolean> predicate)
        {
            return db.Products.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product != null)
                db.Products.Remove(product);
        }

        #region Функции множеств

        public List<string> GetMarks(string type)
        {
            switch (type)
            {
                case "Hookah":
                    return db.Products.OfType<Hookah>().Select(c => c.Mark).ToList();
                case "HookahTobacco":
                    return db.Products.OfType<HookahTobacco>().Select(c => c.Mark).ToList();
                default:
                    return null;
            }
        }

        public async Task<List<string>> GetMarksAsync(string type)
        {
            switch (type)
            {
                case "Hookah":
                    return await db.Products.OfType<Hookah>().Select(c => c.Mark).ToListAsync();
                case "HookahTobacco":
                    return await db.Products.OfType<HookahTobacco>().Select(c => c.Mark).ToListAsync();
                default:
                    return null;
            }
        }

        public IQueryable<Hookah> GetHookahs()
        {
            return db.Products.OfType<Hookah>().AsQueryable();
        }

        #endregion


        #region Агрегатные функции
        public int GetMinPrice()
        {
            return db.Products.Select(p => p.Price).Min();
        }

        public async Task<int> GetMinPriceAsync()
        {
            return await db.Products.Select(p => p.Price).MinAsync();
        }

        public int GetMaxPrice()
        {
            return db.Products.Select(p => p.Price).Max();
        }

        public async Task<int> GetMaxPriceAsync()
        {
            return await db.Products.Select(p => p.Price).MaxAsync();
        }
        #endregion
    }
}
