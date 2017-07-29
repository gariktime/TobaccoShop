using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        void Create(ShopUser item);

        ShopUser FindById(string id);
        Task<ShopUser> FindByIdAsync(string id);

        Task<List<ShopUser>> GetAllAsync();
        Task<List<ShopUser>> GetAllAsync(Expression<Func<ShopUser, bool>> predicate);
    }
}
