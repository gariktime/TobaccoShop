using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);

        ClientProfile FindById(string id);
        Task<ClientProfile> FindByIdAsync(string id);

        Task<List<ClientProfile>> GetAllAsync();
        Task<List<ClientProfile>> GetAllAsync(Expression<Func<ClientProfile, bool>> predicate);
    }
}
