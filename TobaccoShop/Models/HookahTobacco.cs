using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class HookahTobacco : Product
    {
        /// <summary>
        /// Вес табака в граммах.
        /// </summary>
        [Required(ErrorMessage ="Введите вес табака в граммах.")]
        [Range(1, 5000, ErrorMessage = "Вес табака должен быть от 1 до 5000 грамм")]
        public double Weight { get; set; }

        public HookahTobacco(string mark, string model, double weight, decimal price, int available)
            : base(mark, model, price, available)
        {
            this.Weight = weight;
        }
    }
}