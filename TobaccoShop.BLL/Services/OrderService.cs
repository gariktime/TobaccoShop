using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Identity;
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
                    ClientProfile clientProfile = await db.ClientManager.FindByIdAsync(orderDTO.User.Id);
                    Mapper.Initialize(cfg => cfg.CreateMap<List<OrderedProductDTO>, List<OrderedProduct>>());
                    List<OrderedProduct> orderedProducts = Mapper.Map<List<OrderedProductDTO>, List<OrderedProduct>>(orderDTO.Products);

                    Order order = new Order()
                    {
                        OrderId = Guid.NewGuid(),
                        OrderPrice = orderDTO.OrderPrice,
                        Products = orderedProducts,
                        User = clientProfile,
                        OrderDate = orderDTO.OrderDate,
                        Street = orderDTO.Street.Trim(),
                        House = orderDTO.House.Trim(),
                        Apartment = orderDTO.Apartment.Trim(),
                        Note = (orderDTO.Note == null || orderDTO.Note.Trim() == "") ? null : orderDTO.Note,
                        PhoneNumber = orderDTO.PhoneNumber
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
    }
}
