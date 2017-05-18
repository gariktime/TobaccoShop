using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.DAL.Entities.Products
{
    public abstract class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Введите марку товара")]
        public string Mark { get; set; }

        [Required(ErrorMessage = "Введите модель товара")]
        public string Model { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Описание товара должно быть от 5 до 2000 символов")]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [Range(0, 5000)]
        public int Available { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Country { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Product()
        {
            Comments = new List<Comment>();
        }

        public Product(string mark, string model, int price, int available, string description, string country)
        {
            Mark = mark;
            Model = model;
            Price = price;
            Available = available;
            Country = country;
            Description = description;
            Comments = new List<Comment>();
        }
    }
}