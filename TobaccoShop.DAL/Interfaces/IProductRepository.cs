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

        List<Product> GetAll();
        List<Product> GetAll(Func<Product, bool> predicate);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate);
    }
}
