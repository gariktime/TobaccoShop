using System;

namespace TobaccoShop.BLL.DTO
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        public string Mark { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public int Price { get; set; }

        public string Image { get; set; }
    }
}
