using System.Collections.Generic;
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

        public Hookah(string mark, string model, int price, int available, string description, string country, double height, byte[] image)
            : base(mark, model, price, available, description, country, image)
        {
            Height = height;
        }
    }
}