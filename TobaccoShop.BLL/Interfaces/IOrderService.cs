using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OperationDetails> AddOrder(OrderDTO orderDTO);
        Task<OperationDetails> DeleteOrder(Guid orderId);

        Task<OperationDetails> ChangeOrderStatus(Guid orderId, string newStatus);

        Task<OrderDTO> FindByIdAsync(Guid orderId);
        Task<OrderDTO> FindByNumberAsync(int orderNumber);
        
        Task<List<OrderDTO>> GetOrdersAsync();
        Task<List<OrderDTO>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo);
        Task<List<OrderDTO>> GetActiveOrdersAsync();
        Task<List<OrderDTO>> GetActiveOrdersAsync(DateTime dateFrom, DateTime dateTo);
        Task<List<OrderDTO>> GetOnDeliveryOrdersAsync();
        Task<List<OrderDTO>> GetCompletedOrdersAsync();

        void Dispose();
    }
}
