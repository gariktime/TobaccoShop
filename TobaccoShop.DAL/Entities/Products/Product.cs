using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

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

        public byte[] Image { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Product()
        {
            Comments = new List<Comment>();
        }

        public Product(string mark, string model, int price, int available, string description, string country, byte[] image)
        {
            Mark = mark;
            Model = model;
            Price = price;
            Available = available;
            Country = (country == null || country == "") ? "Нет данных" : country;
            Description = (description == null || description == "") ? "Отсутствует" : description;
            //если передаём null, то если Image null ставим изображение по умолчанию, иначе не меняем его
            Image = (image == null) ? (Image == null) ? GetDefaultImage() : Image : image;
            Comments = new List<Comment>();
        }

        private byte[] GetDefaultImage()
        {
            //byte[] imageData = File.ReadAllBytes(@"C:\Users\Admin\Documents\Downloads\Default.jpg");

            string hex = "0xFFD8FFE000104A46494600010100000100010000FFDB0043000403030403030404030405040405060A07060606060D090A080A0F0D10100F0D0F0E11131814111217120E0F151C151719191B1B1B10141D1F1D1A1F181A1B1AFFDB0043010405050605060C07070C1A110F111A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A1A";
            var data = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(hex);
            

            return data.Value;
        }
    }
}