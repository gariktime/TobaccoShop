using System.Collections.Generic;
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

        public Hookah()
        {

        }

        public Hookah(string mark, string model, double height, int price, int available)
            : base(mark, model, price, available)
        {
            this.Height = height;
        }
    }

    public class HookahListViewModel
    {
        [Required(ErrorMessage = "Введите минимальную цену")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public int minPrice { get; set; }

        [Required(ErrorMessage = "Введите максимальную цену")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public int maxPrice { get; set; }

        [Required(ErrorMessage = "Введите минимальную высоту")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public double minHeight { get; set; }

        [Required(ErrorMessage = "Введите максимальную высоту")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public double maxHeight { get; set; }

        public IEnumerable<Hookah> Products;

        public List<MarkItem> Marks;

        public HookahListViewModel()
        {

        }


    }

    public class MarkItem
    {
        public string Name { get; set; }
        public bool Value { get; set; }

        public MarkItem(string name, bool value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}