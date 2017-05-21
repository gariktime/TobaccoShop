using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IProductService
    {
        Task<OperationDetails> AddHookah(HookahDTO hookahDto);
        Task<OperationDetails> AddHookahTobacco(HookahTobaccoDTO hookahTobaccoDto);

        List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
        Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
    }
}
