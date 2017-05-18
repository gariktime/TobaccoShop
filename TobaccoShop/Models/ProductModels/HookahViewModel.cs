using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models.ProductModels
{
    public class HookahViewModel
    {
        [Required(ErrorMessage = "Введите марку товара")]
        [Display(Name = "Марка")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Некорректные данные")]
        public string Mark { get; set; }

        [Required(ErrorMessage = "Введите модель товара")]
        [Display(Name = "Модель")]
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
        [Display(Name = "Цена")]
        [Range(1, 999999, ErrorMessage = "Некорректная цена")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Введите количество товара")]
        [Display(Name = "Доступно")]
        [Range(0, 99999, ErrorMessage = "Некорректное количество товара")]
        public int Available { get; set; }

        [Required(ErrorMessage = "Введите высоту кальяна в сантиметрах")]
        [Display(Name = "Высота в сантиметрах")]
        [Range(10, 200, ErrorMessage = "Некорректная высота кальяна")]
        public double Height { get; set; }
    }
}
