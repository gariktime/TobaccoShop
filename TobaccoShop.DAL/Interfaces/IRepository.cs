using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);

        T FindById(Guid id);
        Task<T> FindByIdAsync(Guid id);

        List<T> GetAll();
        List<T> GetAll(Func<T, bool> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
