using System;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.DAL.Entities.Products
{
    public class HookahTobacco : Product
    {
        /// <summary>
        /// Вес табака в граммах.
        /// </summary>
        [Range(1, 10000)]
        public double Weight { get; set; }

        public HookahTobacco()
        {

        }

        public HookahTobacco(Guid Id, string mark, string model, int price, string description, string country, double weight, string image)
            : base(Id, mark, model, price, description, country, image)
        {
            Weight = weight;
        }
    }
}
