using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models.ProductListModels
{
    public class HookahListViewModel
    {
        [Required(ErrorMessage = "Введите минимальную цену")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public int MinPrice { get; set; }

        [Required(ErrorMessage = "Введите максимальную цену")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public int MaxPrice { get; set; }

        [Required(ErrorMessage = "Введите минимальную высоту")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public double MinHeight { get; set; }

        [Required(ErrorMessage = "Введите максимальную высоту")]
        [Range(1, 999999, ErrorMessage = "Неверные данные")]
        public double MaxHeight { get; set; }

        public List<string> Marks { get; set; }

        public List<string> Countries { get; set; }

        public string[] SelectedMarks { get; set; }

        public string[] SelectedCountries { get; set; }


    }
}
