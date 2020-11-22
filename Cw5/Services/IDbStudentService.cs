﻿using System.Collections.Generic;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.DTOs.ResultContainers;
using Cw5.Models;

namespace Cw5.Services
{
    public interface IDbStudentService
    {
        public IEnumerable<Student> GetAllStudents();
        public GetSingleStudentResponse GetStudent(string indexNumber);
        public EnrollmentResult EnrollStudent(EnrollStudentRequest newStudent);
    }
}