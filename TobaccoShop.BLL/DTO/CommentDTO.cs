using System;

namespace TobaccoShop.BLL.DTO
{
    public class CommentDTO
    {
        public Guid CommentId { get; set; }

        public Guid ProductId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CommentDate { get; set; }

        public string Text { get; set; }
    }
}
