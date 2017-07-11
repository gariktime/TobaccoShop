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
    public class ClientManager : IClientManager
    {
        public ApplicationContext db { get; set; }

        public ClientManager(ApplicationContext context)
        {
            db = context;
        }

        public void Create(ClientProfile item)
        {
            db.ClientProfiles.Add(item);
        }

        public ClientProfile FindById(string id)
        {
            return db.ClientProfiles.Include("Orders.Products.Product").FirstOrDefault(p => p.Id == id);
        }

        public async Task<ClientProfile> FindByIdAsync(string id)
        {
            return await db.ClientProfiles.Include("Orders.Products.Product").FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ClientProfile>> GetAllAsync()
        {
            return await db.ClientProfiles.Include("Orders.Products.Product").AsNoTracking().ToListAsync();
        }

        public async Task<List<ClientProfile>> GetAllAsync(Expression<Func<ClientProfile, bool>> predicate)
        {
            return await db.ClientProfiles.Include("Orders.Products.Product").Where(predicate).AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
