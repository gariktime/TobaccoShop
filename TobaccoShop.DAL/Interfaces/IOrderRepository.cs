using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order item);
        void Update(Order item);
        void Delete(Guid orderId);

        Order FindById(Guid orderId);
        Task<Order> FindByIdAsync(Guid orderId);
        Order FindByNumber(int orderNumber);
        Task<Order> FindByNumberAsync(int orderNumber);

        List<Order> GetAll();
        List<Order> GetAll(Func<Order, bool> predicate);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate);
        Task<List<Order>> GetUserOrdersAsync(string userId);
    }
}
