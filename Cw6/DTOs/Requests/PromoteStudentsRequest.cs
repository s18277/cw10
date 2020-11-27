using System.ComponentModel.DataAnnotations;

namespace Cw6.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        [Required(ErrorMessage = "Nazwa kierunku jest wymagana do promocji studentów!")]
        [MaxLength(100)]
        public string Studies { get; set; }

        [Required(ErrorMessage = "Numer semestru jest wymagany do promocji studentów!")]
        public int Semester { get; set; }
    }
}