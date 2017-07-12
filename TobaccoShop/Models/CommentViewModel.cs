using System;
using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class CommentViewModel
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Введите текст комментария.")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Длина сообщения должна быть от 5 до 500 символов.")]
        public string Text { get; set; }
    }
}
