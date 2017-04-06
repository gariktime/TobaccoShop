using System;
using System.Collections.Generic;

namespace TobaccoShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrederInfo> Products { get; set; }

        public Order()
        {
            Products = new List<OrederInfo>();
        }
    }
}
