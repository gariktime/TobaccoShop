using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OperationDetails> AddOrder(OrderDTO orderDTO);

        Task<List<OrderDTO>> GetOrdersAsync();
        Task<List<OrderDTO>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo);
        Task<List<OrderDTO>> GetActiveOrdersAsync();
        Task<List<OrderDTO>> GetActiveOrdersAsync(DateTime dateFrom, DateTime dateTo);
    }
}
