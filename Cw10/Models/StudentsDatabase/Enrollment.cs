﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Cw10.Models.StudentsDatabase
{
    public class Enrollment
    {
        public Enrollment()
        {
            Students = new HashSet<Student>();
        }

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Study IdStudyNavigation { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}