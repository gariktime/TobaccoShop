using System.Collections.Generic;
using TobaccoShop.BLL.DTO;

namespace TobaccoShop.Models
{
    public class CartViewModel
    {
        public List<OrderedProductDTO> Products { get; set; }

        public double TotalPrice { get; set; }
    }
}
