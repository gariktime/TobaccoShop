using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext db { get; set; }

        public UserRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Create(ShopUser item)
        {
            db.ShopUsers.Add(item);
        }

        public ShopUser FindById(string id)
        {
            return db.ShopUsers.Include(p => p.Orders).FirstOrDefault(p => p.Id == id);
        }

        public async Task<ShopUser> FindByIdAsync(string id)
        {
            return await db.ShopUsers.Include(p => p.Orders).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ShopUser>> GetAllAsync()
        {
            return await db.ShopUsers.Include(p => p.Orders).AsNoTracking().ToListAsync();
        }

        public async Task<List<ShopUser>> GetAllAsync(Expression<Func<ShopUser, bool>> predicate)
        {
            return await db.ShopUsers.Include(p => p.Orders).Where(predicate).AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
