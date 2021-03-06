﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        void Update(TEntity item);

        List<TEntity> GetProducts();
        List<TEntity> GetProducts(Func<TEntity, bool> predicate);
        Task<List<TEntity>> GetProductsAsync();
        Task<List<TEntity>> GetProductsAsync(Expression<Func<TEntity,bool>> predicate);

        List<X> GetPropValues<X>(Func<TEntity, X> selector);
        Task<List<X>> GetPropValuesAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMinValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMinValueAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMaxValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMaxValueAsync<X>(Expression<Func<TEntity, X>> selector);
    }
}
