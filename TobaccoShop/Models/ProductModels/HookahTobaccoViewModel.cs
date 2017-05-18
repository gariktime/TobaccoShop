using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models.ProductModels
{
    public class HookahTobaccoViewModel
    {
        [Required(ErrorMessage = "Введите марку товара")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Некорректные данные")]
        public string Mark { get; set; }

        [Required(ErrorMessage = "Введите модель товара")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Некорректные данные")]
        public string Model { get; set; }

        [Display(Name = "Описание товара")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        [StringLength(25, MinimumLength = 2)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Range(1, 999999, ErrorMessage = "Некорректная цена")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Введите количество товара")]
        [Range(0, 99999, ErrorMessage = "Некорректное количество товара")]
        public int Available { get; set; }

        [Required(ErrorMessage = "Введите вес табака в граммах")]
        [Range(1, 10000, ErrorMessage = "Некорректный вес табака")]
        public double Weight { get; set; }
    }
}
