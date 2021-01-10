using System;
using System.ComponentModel.DataAnnotations;

namespace Cw10.DTOs.Requests
{
    public class UpdateStudentRequest
    {
        [RegularExpression("^s[\\d]+$")] public string IndexNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? IdEnrollment { get; set; }
    }
}