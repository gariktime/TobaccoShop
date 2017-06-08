using System;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IClientManager: IDisposable
    {
        void Create(ClientProfile item);
        ClientProfile FindById(string id);
        Task<ClientProfile> FindByIdAsync(string id);
    }
}
