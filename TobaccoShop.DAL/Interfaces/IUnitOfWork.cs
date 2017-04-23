using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Repositories;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Hookah> Hookahs { get; }

        OrderRepository Orders { get; } //хуйня
        void Save();
    }
}
