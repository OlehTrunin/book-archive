using System.ComponentModel.DataAnnotations;

namespace book_archive.ViewModel
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