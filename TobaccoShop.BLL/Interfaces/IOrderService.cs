using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OperationDetails> AddOrder(OrderDTO orderDTO);
    }
}
