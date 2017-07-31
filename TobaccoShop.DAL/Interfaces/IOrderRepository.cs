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

        List<Order> GetOrders();
        List<Order> GetOrders(Func<Order, bool> predicate);
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
        Task<List<Order>> GetUserOrdersAsync(string userId);

        Task<int> GetOrdersCountAsync(Expression<Func<Order, bool>> predicate);

        Task<double> GetOrderPriceMinAsync(Expression<Func<Order, bool>> predicate);
        Task<double> GetOrderPriceMaxAsync(Expression<Func<Order, bool>> predicate);
        Task<double> GetOrderPriceAverageAsync(Expression<Func<Order, bool>> predicate);

        Task<double> GetOrderProductsCountAsync(Expression<Func<Order, bool>> predicate);
        Task<double> GetOrderProductsCountDistinctAsync(Expression<Func<Order, bool>> predicate);
    }
}
