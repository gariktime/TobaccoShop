using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork db;

        public ProductService(IUnitOfWork uow)
        {
            db = uow;
        }

        public List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return db.Hookahs.GetList(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight);
            else
                return db.Hookahs.GetList(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight &&
                                               marks.Contains(p.Mark));
        }

        public async Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return await db.Hookahs.GetListAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight);
            else
                return await db.Hookahs.GetListAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight &&
                                                          marks.Contains(p.Mark));
        }
    }
}
