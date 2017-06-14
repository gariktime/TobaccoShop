using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

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

        public byte[] Image { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Product()
        {
            RowVersion = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Comments = new List<Comment>();
        }

        public Product(Guid Id, string mark, string model, int price, string description, string country, byte[] image)
        {
            ProductId = Id;
            Mark = mark;
            Model = model;
            Price = price;
            Country = (country == null || country == "") ? "Нет данных" : country;
            Description = (description == null || description == "") ? "Отсутствует" : description;
            //если передаём null, то если Image null ставим изображение по умолчанию, иначе не меняем его
            Image = (image == null) ? (Image == null) ? GetDefaultImage() : Image : image;
            RowVersion = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Comments = new List<Comment>();
        }

        private byte[] GetDefaultImage()
        {
            byte[] imageData = File.ReadAllBytes(@"C:\Users\Admin\Documents\Downloads\Default.jpg");

            //string hex = "0xFFD8FFE000104A46494600010100000100010000FFDB0043000403030403030404030405040405060A07060606060D090A080A0F0D10100F0D0F0E11131814111217120E0F151C151719191B1B1B10141D1F1D1A1F181A1B1AFFDB0043010405050605060C07070C1A110F111A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A";
            //var data = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(hex);

            return imageData;
        }
    }
}