using System;

namespace Cw10.DTOs.Responses
{
    public class GetAllStudentsStudentResponse
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
    }
}