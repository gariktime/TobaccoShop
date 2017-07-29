using System;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }

        public double OrderPrice { get; set; }

        [Required]
        [Display(Name = "Обращение")]
        [StringLength(75, MinimumLength = 2, ErrorMessage = "Длина поля должна быть от 2 до 75 символов")]
        public string Appeal { get; set; }

        [Required]
        [Display(Name = "Улица")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина поля должна быть от 2 до 50 символов")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Дом")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 25 символов")]
        public string House { get; set; }

        [Required]
        [Display(Name = "Квартира")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 2 до 75 символов")]
        public string Apartment { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Длина поля должна быть до 500 символов")]
        public string Note { get; set; }
    }
}
