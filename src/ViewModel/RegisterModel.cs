using System.ComponentModel.DataAnnotations;

namespace book_archive.ViewModel
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказано електронну скриньку")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не вказано пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введено невірно")]
        public string ConfirmPassword { get; set; }
    }
}