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

        public HookahTobacco(string mark, string model, int price, int available, string description, string country, double weight, byte[] image)
            : base(mark, model, price, available, description, country, image)
        {
            Weight = weight;
        }
    }
}
