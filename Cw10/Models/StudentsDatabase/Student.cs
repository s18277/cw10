using System;
using System.Collections.Generic;

#nullable disable

namespace Cw10.Models.StudentsDatabase
{
    public class Student
    {
        public Student()
        {
            RoleStudents = new HashSet<RoleStudent>();
        }

        public int IdStudent { get; set; }
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string SaltPasswordHash { get; set; }
        public string RefreshToken { get; set; }
        public int IdEnrollment { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }
        public virtual ICollection<RoleStudent> RoleStudents { get; set; }
    }
}