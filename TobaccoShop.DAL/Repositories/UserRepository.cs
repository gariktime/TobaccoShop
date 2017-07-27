﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext db { get; set; }

        public UserRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Create(ClientProfile item)
        {
            db.ClientProfiles.Add(item);
        }

        public ClientProfile FindById(string id)
        {
            return db.ClientProfiles.Include(p => p.Orders).FirstOrDefault(p => p.Id == id);
        }

        public async Task<ClientProfile> FindByIdAsync(string id)
        {
            return await db.ClientProfiles.Include(p => p.Orders).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ClientProfile>> GetAllAsync()
        {
            return await db.ClientProfiles.Include(p => p.Orders).AsNoTracking().ToListAsync();
        }

        public async Task<List<ClientProfile>> GetAllAsync(Expression<Func<ClientProfile, bool>> predicate)
        {
            return await db.ClientProfiles.Include(p => p.Orders).Where(predicate).AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}