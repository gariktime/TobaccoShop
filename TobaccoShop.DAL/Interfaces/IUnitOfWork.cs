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
        ProductRepository Products { get; }
        OrderRepository Orders { get; }
        void Save();
    }
}
