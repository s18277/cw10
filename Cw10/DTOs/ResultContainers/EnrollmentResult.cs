using Cw10.DTOs.Responses;
using Cw10.Models.StudentsDatabase;

namespace Cw10.DTOs.ResultContainers
{
    public class EnrollmentResult
    {
        public bool Successful { get; set; }
        public Student Student { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public string Error { get; set; }
    }
}