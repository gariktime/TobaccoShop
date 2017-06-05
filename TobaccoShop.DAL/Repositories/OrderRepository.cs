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
    public class OrderRepository : IRepository<Order>
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

        public void Delete(Order order)
        {
            db.Orders.Remove(order);
            db.Entry(order).State = EntityState.Deleted;
        }

        public Order FindById(Guid id)
        {
            return db.Orders.Find(id);
        }

        public async Task<Order> FindByIdAsync(Guid id)
        {
            return await db.Orders.FindAsync(id);
        }

        public List<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        public List<Order> GetAll(Func<Order, bool> predicate)
        {
            return db.Orders.Where(predicate).ToList();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await db.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Where(predicate).ToListAsync();
        }
    }
}
