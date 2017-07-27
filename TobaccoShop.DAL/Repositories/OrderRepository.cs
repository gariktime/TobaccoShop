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
            return db.Orders.FirstOrDefault(p => p.Number == orderNumber);
        }

        public async Task<Order> FindByNumberAsync(int orderNumber)
        {
            return await db.Orders.FirstOrDefaultAsync(p => p.Number == orderNumber);
        }

        public List<Order> GetAll()
        {
            return db.Orders.Include(p => p.User).Include("Products.Product").ToList();
        }

        public List<Order> GetAll(Func<Order, bool> predicate)
        {
            return db.Orders.Include(p => p.User).Include("Products.Product").Where(predicate).ToList();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await db.Orders.Include(p => p.User).Include("Products.Product").ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Include(p => p.User).Include("Products.Product").Where(predicate).ToListAsync();
        }
    }
}
