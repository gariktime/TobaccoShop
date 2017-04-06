using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        [Required(ErrorMessage ="Введите текст комментария")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Комментарий должен быть длиной от 5 до 500 символов")]
        public string Text { get; set; }

        public int? ProductID { get; set; }
        public Product Product { get; set; }
    }
}
