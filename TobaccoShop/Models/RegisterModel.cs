using System.ComponentModel.DataAnnotations;

namespace TobaccoShop.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "Длина имени пользователя должна быть от 3 до 25 символов", MinimumLength = 3)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Некорректный E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль должен быть длиной минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
    }
}
