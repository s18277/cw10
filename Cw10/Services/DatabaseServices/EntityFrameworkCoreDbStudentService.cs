using System;
using System.Collections.Generic;
using System.Linq;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;
using Cw10.Models.StudentsDatabase;
using Microsoft.EntityFrameworkCore;

namespace Cw10.Services.DatabaseServices
{
    public class EntityFrameworkCoreDbStudentService : IDbStudentService
    {
        private readonly StudentsDbContext _dbContext;

        public EntityFrameworkCoreDbStudentService(StudentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<GetAllStudentsStudentResponse> GetAllStudents()
        {
            return _dbContext.Students.Select(student => new GetAllStudentsStudentResponse
            {
                IndexNumber = student.IndexNumber,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                IdEnrollment = student.IdEnrollment
            }).ToList();
        }

        public GetSingleStudentResponse GetStudent(string indexNumber)
        {
            return _dbContext.Students
                .Where(student => student.IndexNumber == indexNumber)
                .Include(student => student.IdEnrollmentNavigation)
                .ThenInclude(enrollment => enrollment.IdStudyNavigation)
                .Select(student => new GetSingleStudentResponse
                {
                    IndexNumber = student.IndexNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    BirthDate = student.BirthDate,
                    Name = student.IdEnrollmentNavigation.IdStudyNavigation.Name,
                    Semester = student.IdEnrollmentNavigation.Semester,
                    StartDate = student.IdEnrollmentNavigation.StartDate
                }).FirstOrDefault();
        }

        public EnrollmentResult EnrollStudent(EnrollStudentRequest newStudent)
        {
            throw new NotImplementedException();
        }

        public EnrollmentDto PromoteStudents(PromoteStudentsRequest promoteStudentsRequest)
        {
            throw new NotImplementedException();
        }

        public SingleStudentAuthenticationData GetStudentsAuthenticationData(string indexNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRefreshToken(string username, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}