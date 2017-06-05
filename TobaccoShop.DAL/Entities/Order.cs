using System;
using System.Collections.Generic;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserId { get; set; }
        public ClientProfile User { get; set; }

        public ICollection<OrderedProduct> Products { get; set; }

        public Order()
        {
            Products = new List<OrderedProduct>();
        }
    }
}
