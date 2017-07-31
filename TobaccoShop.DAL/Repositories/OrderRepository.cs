using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationContext db;

        public OrderRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Add(Order order)
        {
            db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
        }

        public void Delete(Guid orderId)
        {
            Order order = db.Orders.Find(orderId);
            if (order != null)
                db.Orders.Remove(order);
            else
                throw new ArgumentException("Заказ с указанным Id не найден.");
        }

        public Order FindById(Guid orderId)
        {
            return db.Orders.Include(p => p.User).Include("Products.Product").FirstOrDefault(p => p.OrderId == orderId);
        }

        public async Task<Order> FindByIdAsync(Guid orderId)
        {
            return await db.Orders.Include(p => p.User).Include("Products.Product").FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public Order FindByNumber(int orderNumber)
        {
            return db.Orders.Include(p => p.User).FirstOrDefault(p => p.Number == orderNumber);
        }

        public async Task<Order> FindByNumberAsync(int orderNumber)
        {
            return await db.Orders.Include(p => p.User).FirstOrDefaultAsync(p => p.Number == orderNumber);
        }

        public List<Order> GetOrders()
        {
            return db.Orders.AsNoTracking().Include(p => p.User).ToList();
        }

        public List<Order> GetOrders(Func<Order, bool> predicate)
        {
            return db.Orders.AsNoTracking().Include(p => p.User).Where(predicate).ToList();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await db.Orders.AsNoTracking().Include(p => p.User).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.AsNoTracking().Include(p => p.User).Where(predicate).ToListAsync();
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await db.Orders.Include("Products.Product").AsNoTracking().ToListAsync();
        }

        public async Task<int> GetOrdersCountAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.CountAsync(predicate);
        }

        public async Task<double> GetOrderPriceMinAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Where(predicate).Select(p => p.OrderPrice).DefaultIfEmpty(0.0).MinAsync();
        }

        public async Task<double> GetOrderPriceMaxAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Where(predicate).Select(p => p.OrderPrice).DefaultIfEmpty(0.0).MaxAsync();
        }

        public async Task<double> GetOrderPriceAverageAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Where(predicate).Select(p => p.OrderPrice).DefaultIfEmpty(0.0).AverageAsync();
        }

        public async Task<double> GetOrderProductsCountAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Include(p => p.Products).Where(predicate)
                           .Select(p => p.Products.Sum(c => c.Quantity)).DefaultIfEmpty(0).AverageAsync();
        }

        public async Task<double> GetOrderProductsCountDistinctAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Include(p => p.Products).Where(predicate)
                           .Select(p => p.Products.Count).DefaultIfEmpty(0).AverageAsync();
        }
    }
}
