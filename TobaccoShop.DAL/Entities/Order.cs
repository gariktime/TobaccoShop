﻿using System;
using System.Collections.Generic;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public double OrderPrice { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public string PhoneNumber { get; set; }

        public string Note { get; set; }

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
