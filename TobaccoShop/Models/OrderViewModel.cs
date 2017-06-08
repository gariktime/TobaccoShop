using System;

namespace TobaccoShop.Models
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }

        public double OrderPrice { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public string PhoneNumber { get; set; }

        public string Note { get; set; }
    }
}