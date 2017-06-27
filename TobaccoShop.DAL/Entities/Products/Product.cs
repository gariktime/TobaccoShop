using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.DAL.Entities.Products
{
    //Этот класс очень хотел быть абстрактным,
    //но AutoMapper оказался против
    public class Product
    {
        public Guid ProductId { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Mark { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Model { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Описание товара должно быть от 5 до 2000 символов")]
        public string Description { get; set; }

        [Range(1, 999999)]
        public int Price { get; set; }

        [StringLength(25, MinimumLength = 2)]
        public string Country { get; set; }

        public string Image { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Product()
        {
            RowVersion = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Comments = new List<Comment>();
        }

        public Product(Guid Id, string mark, string model, int price, string description, string country, string image)
        {
            ProductId = Id;
            Mark = mark;
            Model = model;
            Price = price;
            Country = country;
            Description = description;
            Image = image;
            RowVersion = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Comments = new List<Comment>();
        }
    }
}