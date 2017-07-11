using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Services;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IProductService
    {
        //создание новых продуктов
        Task<OperationDetails> AddHookah(HookahDTO hookahDto);
        Task<OperationDetails> AddHookahTobacco(HookahTobaccoDTO hookahTobaccoDto);

        //редактирование продуктов
        Task<OperationDetails> EditHookah(HookahDTO hookahDto);

        //удаление продукта
        Task<OperationDetails> RemoveProduct(Guid id);

        //методы множеств
        ProductDTO FindById(Guid id);
        Task<ProductDTO> FindByIdAsync(Guid id);
        List<ProductDTO> GetProducts();
        List<ProductDTO> GetProducts(Func<ProductDTO, bool> predicate);
        Task<List<ProductDTO>> GetProductsAsync();
        Task<List<ProductDTO>> GetProductsAsync(Func<ProductDTO, bool> predicate);
        Task<List<HookahDTO>> GetHookahsAsync();
        Task<List<HookahDTO>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks, string[] countries);

        //property methods
        Task<(int, int, double, double, List<string>, List<string>)> GetHookahProperties();

        //вспомогательные методы
        Task<(ProductDTO, ProductType)> GetProductParamsAsync(Guid id);

        void Dispose();
    }
}
