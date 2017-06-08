using System;

namespace TobaccoShop.BLL.DTO
{
    public class OrderedProductDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string MarkModel { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int LinePrice { get { return Price * Quantity; } }
    }
}
