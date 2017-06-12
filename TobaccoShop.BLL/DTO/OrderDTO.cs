using System;
using System.Collections.Generic;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.BLL.DTO
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public double OrderPrice { get; set; }

        public string Appeal { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public string PhoneNumber { get; set; }

        public string Note { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public string UserId { get; set; }

        public UserDTO User { get; set; }

        public List<OrderedProductDTO> Products { get; set; }
    }
}
