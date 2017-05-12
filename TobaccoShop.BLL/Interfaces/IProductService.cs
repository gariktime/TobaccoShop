using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IProductService
    {
        List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
        Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
    }
}
