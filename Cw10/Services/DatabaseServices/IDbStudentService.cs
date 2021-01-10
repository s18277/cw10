﻿using System.Collections.Generic;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;

namespace Cw10.Services.DatabaseServices
{
    public interface IDbStudentService
    {
        public IEnumerable<GetAllStudentsStudentResponse> GetAllStudents();
        public GetSingleStudentResponse GetStudent(string indexNumber);
        public EnrollmentResult EnrollStudent(EnrollStudentRequest newStudent);
        public EnrollmentDto PromoteStudents(PromoteStudentsRequest promoteStudentsRequest);
        public SingleStudentAuthenticationData GetStudentsAuthenticationData(string indexNumber);
        public bool UpdateRefreshToken(string username, string refreshToken);
    }
}