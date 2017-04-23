using System;
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
        List<TEntity> GetList();
        List<TEntity> GetList(Func<TEntity, bool> predicate);
        Task<List<TEntity>> GetListAsync();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity,bool>> predicate);
        TEntity FindById(int id);
        List<X> GetPropValues<X>(Func<TEntity, X> selector);
        Task<List<X>> GetPropValuesAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMinValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMinValueAsync<X>(Expression<Func<TEntity, X>> selector);
        X GetPropMaxValue<X>(Func<TEntity, X> selector);
        Task<X> GetPropMaxValueAsync<X>(Expression<Func<TEntity, X>> selector);
    }
}
