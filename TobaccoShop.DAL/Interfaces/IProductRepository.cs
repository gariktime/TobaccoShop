using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IProductRepository
    {
        void Delete(Guid productId);

        Product FindById(Guid productId);
        Task<Product> FindByIdAsync(Guid productId);

        List<Product> GetProducts();
        List<Product> GetProducts(Func<Product, bool> predicate);
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsAsync(Expression<Func<Product, bool>> predicate);
    }
}
