using System;
using System.Collections.Generic;

namespace TobaccoShop.BLL.DTO
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public int Number { get; set; }

        public double OrderPrice { get; set; }

        public string Appeal { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public string PhoneNumber { get; set; }

        public string Note { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public List<OrderedProductDTO> Products { get; set; }
    }
}
