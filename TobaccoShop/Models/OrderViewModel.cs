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
        public string Appeal { get; set; }

        [Required]
        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Дом")]
        public string House { get; set; }

        [Required]
        [Display(Name = "Квартира")]
        public string Apartment { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
    }
}
