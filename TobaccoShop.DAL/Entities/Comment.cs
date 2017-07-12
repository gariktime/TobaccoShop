using System;
using System.ComponentModel.DataAnnotations;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.DAL.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }

        [StringLength(500)]
        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public ClientProfile User { get; set; }

        public Comment()
        {

        }
    }
}
