﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        List<TEntity> GetAll();
        List<TEntity> GetAll(Func<TEntity, bool> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity,bool>> predicate);
        TEntity FindById(Guid id);
        Task<TEntity> FindByIdAsync(Guid id);
        List<X> GetPropValues<X>(Func<TEntity, X> selector);
        Task<List<X>> GetPropValuesAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMinValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMinValueAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMaxValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMaxValueAsync<X>(Expression<Func<TEntity, X>> selector);
    }
}
