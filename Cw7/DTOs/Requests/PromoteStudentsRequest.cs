using System.ComponentModel.DataAnnotations;

namespace Cw7.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        [Required(ErrorMessage = "Nazwa kierunku jest wymagana do promocji studentów!")]
        public string Studies { get; set; }

        [Required(ErrorMessage = "Numer semestru jest wymagany do promocji studentów!")]
        public int Semester { get; set; }
    }
}