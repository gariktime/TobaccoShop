﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IOrderService : IDisposable
    {
        Task<OperationDetails> AddOrder(OrderDTO orderDTO);
        Task<OperationDetails> DeleteOrder(Guid orderId);

        Task<OperationDetails> ChangeOrderStatus(Guid orderId, string newStatus);

        Task<OrderDTO> FindByIdAsync(Guid orderId);
        Task<OrderDTO> FindByNumberAsync(int orderNumber);

        Task<List<OrderDTO>> GetOrdersAsync();
        Task<List<OrderDTO>> GetActiveOrdersAsync();
        Task<List<OrderDTO>> GetOnDeliveryOrdersAsync();
        Task<List<OrderDTO>> GetCompletedOrdersAsync();
        Task<List<OrderDTO>> GetUserOrdersAsync(string userId);

        Task<(int, int, int)> GetOrderStatusStatistics();
        Task<List<(double, double, double)>> GetOrderPriceStatistics(int year);
        Task<List<int>> GetOrderCountStatistics(int year);
        Task<List<(double, double)>> GetOrderProductsStatistics(int year);
    }
}
