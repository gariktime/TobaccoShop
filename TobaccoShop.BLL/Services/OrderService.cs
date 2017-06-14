﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Util;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork db;

        public OrderService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> AddOrder(OrderDTO orderDTO)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ClientProfile clientProfile = await db.ClientManager.FindByIdAsync(orderDTO.UserId);

                    Mapper.Initialize(cfg => { cfg.AddProfile<AutomapperProfile>(); });
                    List<OrderedProduct> orderedProducts = Mapper.Map<List<OrderedProductDTO>, List<OrderedProduct>>(orderDTO.Products);

                    Order order = new Order()
                    {
                        OrderId = Guid.NewGuid(),
                        OrderPrice = orderDTO.OrderPrice,
                        Products = orderedProducts,
                        User = clientProfile,
                        Appeal = orderDTO.Appeal.Trim(),
                        OrderDate = orderDTO.OrderDate,
                        Street = orderDTO.Street.Trim(),
                        House = orderDTO.House.Trim(),
                        Apartment = orderDTO.Apartment.Trim(),
                        Note = (orderDTO.Note == null || orderDTO.Note.Trim() == "") ? null : orderDTO.Note,
                        PhoneNumber = orderDTO.PhoneNumber,
                        Status = OrderStatus.Active
                    };

                    db.Orders.Add(order);
                    await db.SaveAsync();
                    transaction.Commit();
                    return new OperationDetails(true, "Заказ успешно оформлен", "");
                }
                catch
                {
                    transaction.Rollback();
                    return new OperationDetails(false, "Ошибка при добавлении заказа", "");
                }
            }
        }

        #region Методы множеств

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetAllAsync();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo)
        {
            List<Order> orders = await db.Orders.GetAllAsync(p => p.OrderDate >= dateFrom && p.OrderDate <= dateTo);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetActiveOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetAllAsync(p => p.Status == OrderStatus.Active);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetActiveOrdersAsync(DateTime dateFrom, DateTime dateTo)
        {
            List<Order> orders = await db.Orders.GetAllAsync(p => p.Status == OrderStatus.Active && p.OrderDate >= dateFrom && p.OrderDate <= dateTo);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        #endregion
    }
}