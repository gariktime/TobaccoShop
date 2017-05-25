﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Services;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IProductService
    {
        //создание новых продуктов
        Task<OperationDetails> AddHookah(HookahDTO hookahDto);
        Task<OperationDetails> AddHookahTobacco(HookahTobaccoDTO hookahTobaccoDto);

        //редактирование продуктов
        Task<OperationDetails> EditHookah(Guid id, HookahDTO hookahDto);

        //удаление продукта
        Task<OperationDetails> RemoveProduct(Guid id);

        Product FindById(Guid id);
        Task<Product> FindByIdAsync(Guid id);
        List<Product> GetProducts();
        List<Product> GetProducts(Func<Product, bool> predicate);
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsAsync(Expression<Func<Product, bool>> predicate);
        List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
        Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);

        //вспомогательные методы
        ProductType GetProductType(Product product);
        Hookah ProductAsHookah(Product product);
        HookahTobacco ProductAsHookahTobacco(Product product);
    }
}
