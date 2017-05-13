using System;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IClientManager: IDisposable
    {
        void Create(ClientProfile item);
    }
}
