using System.ComponentModel.DataAnnotations;

namespace Cw10.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana do zalogowania!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane do zalogowania!")]
        public string Password { get; set; }
    }
}