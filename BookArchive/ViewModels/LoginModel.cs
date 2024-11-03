using System.ComponentModel.DataAnnotations;

namespace BookArchive.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не вказано електронну скриньку")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не вказано пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}