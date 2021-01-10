using System.Collections.Generic;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;
using Cw10.Models;

namespace Cw10.Services.DatabaseServices
{
    public interface IDbStudentService
    {
        public IEnumerable<Student> GetAllStudents();
        public GetSingleStudentResponse GetStudent(string indexNumber);
        public EnrollmentResult EnrollStudent(EnrollStudentRequest newStudent);
        public Enrollment PromoteStudents(PromoteStudentsRequest promoteStudentsRequest);
        public SingleStudentAuthenticationData GetStudentsAuthenticationData(string indexNumber);
        public bool UpdateRefreshToken(string username, string refreshToken);
    }
}