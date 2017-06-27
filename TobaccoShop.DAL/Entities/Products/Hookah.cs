using System;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.DAL.Entities.Products
{
    public class Hookah : Product
    {
        /// <summary>
        /// Высота кальяна в сантиметрах.
        /// </summary>
        [Range(10, 200)]
        public double Height { get; set; }

        public Hookah()
        {

        }

        public Hookah(Guid Id, string mark, string model, int price, string description, string country, double height, string image)
            : base(Id, mark, model, price, description, country, image)
        {
            Height = height;
        }
    }
}