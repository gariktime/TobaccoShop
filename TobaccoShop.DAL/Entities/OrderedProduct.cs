using System;
using System.ComponentModel.DataAnnotations.Schema;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.DAL.Entities
{
    public class OrderedProduct
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public OrderedProduct()
        {

        }
    }
}
