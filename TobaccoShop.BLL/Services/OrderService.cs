using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Util;
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

        //добавление заказа в БД
        public async Task<OperationDetails> AddOrder(OrderDTO orderDTO)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //пользователь, оформляющий заказ
                    ShopUser shopUser = await db.Users.FindByIdAsync(orderDTO.UserId);
                    if (shopUser == null)
                        return new OperationDetails(false, "Пользователь с указанным ID не найден", "");

                    //информация о заказываемых товарах
                    Mapper.Initialize(cfg => { cfg.AddProfile<AutomapperProfile>(); });
                    List<OrderedProduct> orderedProducts = Mapper.Map<List<OrderedProductDTO>, List<OrderedProduct>>(orderDTO.Products);

                    //создаём Order из OrderDTO
                    Order order = new Order()
                    {
                        OrderId = Guid.NewGuid(),
                        OrderPrice = orderDTO.OrderPrice,
                        Products = orderedProducts,
                        User = shopUser,
                        Appeal = orderDTO.Appeal.Trim(),
                        OrderDate = DateTime.Now,
                        Street = orderDTO.Street.Trim(),
                        House = orderDTO.House.Trim(),
                        Apartment = orderDTO.Apartment.Trim(),
                        Note = (orderDTO.Note == null || orderDTO.Note.Trim() == "") ? null : orderDTO.Note,
                        PhoneNumber = orderDTO.PhoneNumber,
                        Status = OrderStatus.Active
                    };

                    //добавляем заказ в БД
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

        //удаление заказа
        public async Task<OperationDetails> DeleteOrder(Guid orderId)
        {
            try
            {
                db.Orders.Delete(orderId);
                await db.SaveAsync();
                return new OperationDetails(true, "Заказ успешно удалён.", "");
            }
            catch
            {
                return new OperationDetails(false, "Ошибка при удалении заказа.", "");
            }
        }

        #region Изменение статуса заказа

        public async Task<OperationDetails> ChangeOrderStatus(Guid orderId, string newStatus)
        {
            try
            {
                Order order = await db.Orders.FindByIdAsync(orderId);

                switch (newStatus)
                {
                    case "Active":
                        order.Status = OrderStatus.Active;
                        break;
                    case "OnDelivery":
                        order.Status = OrderStatus.OnDelivery;
                        break;
                    case "Completed":
                        order.Status = OrderStatus.Completed;
                        break;
                    default:
                        return new OperationDetails(false, "Неверный статус заказа", "");
                }

                db.Orders.Update(order);
                await db.SaveAsync();
                return new OperationDetails(true, "Статус заказа успешно изменён", "");
            }
            catch (DbUpdateConcurrencyException)
            {
                return new OperationDetails(false, "Данный заказ ранее был изменён", "");
            }
            catch
            {
                return new OperationDetails(false, "Ошибка при изменении статуса заказа", "");
            }
        }

        #endregion

        #region Поиск заказа по ID и номеру

        public async Task<OrderDTO> FindByIdAsync(Guid orderId)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<Order, OrderDTO>(await db.Orders.FindByIdAsync(orderId));
        }

        public async Task<OrderDTO> FindByNumberAsync(int orderNumber)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<Order, OrderDTO>(await db.Orders.FindByNumberAsync(orderNumber));
        }

        #endregion

        #region Методы множеств

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetOrdersAsync();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetActiveOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetOrdersAsync(p => p.Status == OrderStatus.Active);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetOnDeliveryOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetOrdersAsync(p => p.Status == OrderStatus.OnDelivery);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetCompletedOrdersAsync()
        {
            List<Order> orders = await db.Orders.GetOrdersAsync(p => p.Status == OrderStatus.Completed);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            List<Order> orders = await db.Orders.GetUserOrdersAsync(userId);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        #endregion

        #region Статистика

        //статистика зазазов по статусу
        public async Task<(int, int, int)> GetOrderStatusStatistics()
        {
            int activeOrders = await db.Orders.GetOrdersCountAsync(p => p.Status == OrderStatus.Active);
            int onDeliveryOrders = await db.Orders.GetOrdersCountAsync(p => p.Status == OrderStatus.OnDelivery);
            int completedOrders = await db.Orders.GetOrdersCountAsync(p => p.Status == OrderStatus.Completed);
            return (activeOrders, onDeliveryOrders, completedOrders);
        }

        //статистика стоимости заказов
        public async Task<List<(double, double, double)>> GetOrderPriceStatistics(int year)
        {
            List<(double, double, double)> result = new List<(double, double, double)>();
            for (int i = 1; i <= 12; i++)
            {
                double minPrice = await db.Orders.GetOrderPriceMinAsync(c => c.OrderDate.Month == i && c.OrderDate.Year == year);
                double maxPrice = await db.Orders.GetOrderPriceMaxAsync(c => c.OrderDate.Month == i && c.OrderDate.Year == year);
                double avgPrice = await db.Orders.GetOrderPriceAverageAsync(c => c.OrderDate.Month == i && c.OrderDate.Year == year);
                result.Add((minPrice, maxPrice, avgPrice));
            }
            return result;
        }

        //статистика количества заказов
        public async Task<List<int>> GetOrderCountStatistics(int year)
        {
            List<int> result = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                int ordersCount = await db.Orders.GetOrdersCountAsync(p => p.OrderDate.Month == i && p.OrderDate.Year == year);
                result.Add(ordersCount);
            }
            return result;
        }

        //статистика количества товаров в заказе
        public async Task<List<(double, double)>> GetOrderProductsStatistics(int year)
        {
            List<(double, double)> result = new List<(double, double)>();
            for (int i = 1; i <= 12; i++)
            {
                double productsCountDistinct = await db.Orders.GetOrderProductsCountDistinctAsync(p => p.OrderDate.Month == i && p.OrderDate.Year == year);
                double productsCount = await db.Orders.GetOrderProductsCountAsync(p => p.OrderDate.Month == i && p.OrderDate.Year == year);
                result.Add((productsCountDistinct, productsCount));
            }
            return result;
        }

        #endregion

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
