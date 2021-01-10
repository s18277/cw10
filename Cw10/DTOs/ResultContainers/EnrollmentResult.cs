using Cw10.Models;

namespace Cw10.DTOs.ResultContainers
{
    public class EnrollmentResult
    {
        public bool Successful { get; set; }
        public Student Student { get; set; }
        public Enrollment Enrollment { get; set; }
        public string Error { get; set; }
    }
}