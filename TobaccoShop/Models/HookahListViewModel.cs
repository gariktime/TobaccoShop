using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TobaccoShop.Models
{
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

        public List<string> Marks;

        public string[] SelectedMarks { get; set; }

        public HookahListViewModel()
        {
            this.Marks = new List<string>();
        }
    }
}