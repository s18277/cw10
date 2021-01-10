using System;

namespace Cw10.DTOs.Responses
{
    public class EnrollmentDto
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}