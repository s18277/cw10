using System.ComponentModel.DataAnnotations;

namespace Cw10.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "RefreshToken jest wymagany do aktualizacji JWT!")]
        public string RefreshToken { get; set; }
    }
}