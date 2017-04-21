using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork Db;

        public ProductService(IUnitOfWork uow)
        {
            Db = uow;
        }


    }
}
