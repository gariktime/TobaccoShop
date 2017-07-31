using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        void Add(ShopUser item);

        ShopUser FindById(string id);
        Task<ShopUser> FindByIdAsync(string id);

        Task<List<ShopUser>> GetUsersAsync();
        Task<List<ShopUser>> GetUsersAsync(Expression<Func<ShopUser, bool>> predicate);
    }
}
