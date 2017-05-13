﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Identity;
using TobaccoShop.DAL.Repositories;

namespace TobaccoShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IGenericRepository<Product> Products { get; }
        IGenericRepository<Hookah> Hookahs { get; }

        OrderRepository Orders { get; } //хуйня
        void Save();
        Task SaveAsync();
    }
}