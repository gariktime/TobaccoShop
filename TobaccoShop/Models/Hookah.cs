using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class Hookah : Product
    {
        /// <summary>
        /// Высота кальяна в сантиметрах.
        /// </summary>
        [Required(ErrorMessage = "Введите высоту кальяна")]
        [Range(10, 200, ErrorMessage = "Высота кальяна должна быть от 10 до 200 сантиметров")]
        public double Height { get; set; }

        public Hookah(string mark, string model, double height, decimal price, int available)
            : base(mark, model, price, available)
        {
            this.Height = height;
        }
    }
}