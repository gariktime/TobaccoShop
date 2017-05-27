using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class ProductGRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;

        public ProductGRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region CRUD

        public void Add(TEntity item)
        {
            _dbSet.Add(item);
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
            _context.Entry(item).State = EntityState.Deleted;
        }

        public TEntity FindById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        #endregion

        #region Функции множеств

        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<TEntity> GetAll(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        #endregion

        #region Property functions

        public List<X> GetPropValues<X>(Func<TEntity, X> selector)
        {
            return _dbSet.Select(selector).Distinct().ToList();
        }

        public async Task<List<X>> GetPropValuesAsync<X>(Expression<Func<TEntity, X>> selector)
        {
            return await _dbSet.Select(selector).Distinct().ToListAsync();
        }

        public X GetPropMinValue<X>(Func<TEntity, X> selector)
        {
            return _dbSet.Select(selector).Min();
        }

        public async Task<X> GetPropMinValueAsync<X>(Expression<Func<TEntity, X>> selector)
        {
            return await _dbSet.Select(selector).MinAsync();
        }

        public X GetPropMaxValue<X>(Func<TEntity, X> selector)
        {
            return _dbSet.Select(selector).Max();
        }

        public async Task<X> GetPropMaxValueAsync<X>(Expression<Func<TEntity, X>> selector)
        {
            return await _dbSet.Select(selector).MaxAsync();
        }

        #endregion

    }
}
