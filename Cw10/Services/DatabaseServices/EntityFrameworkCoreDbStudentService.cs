using System;
using System.Collections.Generic;
using System.Linq;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;
using Cw10.Models.StudentsDatabase;
using Cw10.Services.EncryptionServices;
using Microsoft.EntityFrameworkCore;

namespace Cw10.Services.DatabaseServices
{
    public class EntityFrameworkCoreDbStudentService : IDbStudentService
    {
        private readonly StudentsDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;

        public EntityFrameworkCoreDbStudentService(StudentsDbContext dbContext, IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
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
            var studies = _dbContext.Studies.FirstOrDefault(s => s.Name == newStudent.Studies);
            if (studies == null)
                return new EnrollmentResult {Error = $"Kierunek {newStudent.Studies} nie istnieje!"};
            if (_dbContext.Students.Any(s => s.IndexNumber == newStudent.IndexNumber))
                return new EnrollmentResult {Error = $"Numer indeksu {newStudent.IndexNumber} już istnieje!"};
            var enrollment = GetEnrollmentForNewStudent(studies);
            var student = new Student
            {
                IndexNumber = newStudent.IndexNumber,
                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                BirthDate = newStudent.BirthDate,
                SaltPasswordHash = _encryptionService.Encrypt(newStudent.Password)
            };
            enrollment.Students.Add(student);
            _dbContext.Students.Add(student);
            if (_dbContext.SaveChanges() == 0)
                return new EnrollmentResult
                {
                    Successful = false,
                    Error = "Nie zmodyfikowano żadnych wierszy w bazie danych!"
                };
            return new EnrollmentResult
            {
                Successful = true,
                Student = student,
                Enrollment = new EnrollmentDto
                {
                    IdEnrollment = enrollment.IdEnrollment,
                    Semester = enrollment.Semester,
                    IdStudy = enrollment.IdStudy,
                    Name = studies.Name,
                    StartDate = enrollment.StartDate
                }
            };
        }

        public EnrollmentDto PromoteStudents(PromoteStudentsRequest promoteStudentsRequest)
        {
            var studies = promoteStudentsRequest.Studies;
            var semester = promoteStudentsRequest.Semester;
            try {
                return _dbContext.StudentPromotions
                    .FromSqlInterpolated($"PromoteStudents {studies}, {semester}")
                    .AsEnumerable().FirstOrDefault();}
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public SingleStudentAuthenticationData GetStudentsAuthenticationData(string indexNumber)
        {
            return _dbContext.Students
                .Where(student => student.IndexNumber == indexNumber)
                .Include(student => student.RoleStudents)
                .ThenInclude(roleStudent => roleStudent.IdRoleNavigation)
                .Select(student => new SingleStudentAuthenticationData
                {
                    IndexNumber = student.IndexNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    SaltPasswordHash = student.SaltPasswordHash,
                    RefreshToken = student.RefreshToken,
                    Roles = student.RoleStudents.Select(roleStudent => roleStudent.IdRoleNavigation.RoleName).ToArray()
                }).FirstOrDefault();
        }

        public int UpdateRefreshToken(string username, string refreshToken)
        {
            var student = _dbContext.Students.First(s => s.IndexNumber == username);
            student.RefreshToken = refreshToken;
            return _dbContext.SaveChanges();
        }

        public int UpdateStudent(string indexNumber, UpdateStudentRequest updateStudentRequest)
        {
            var student = _dbContext.Students.First(s => s.IndexNumber == indexNumber);
            student.IndexNumber = updateStudentRequest.IndexNumber ?? student.IndexNumber;
            student.FirstName = updateStudentRequest.FirstName ?? student.FirstName;
            student.LastName = updateStudentRequest.LastName ?? student.LastName;
            student.BirthDate = updateStudentRequest.BirthDate ?? student.BirthDate;
            student.IdEnrollment = updateStudentRequest.IdEnrollment ?? student.IdEnrollment;
            return _dbContext.SaveChanges();
        }

        public int DeleteStudent(string indexNumber)
        {
            var student = _dbContext.Students.First(s => s.IndexNumber == indexNumber);
            _dbContext.Remove(student);
            return _dbContext.SaveChanges();
        }

        private Enrollment GetEnrollmentForNewStudent(Study studies)
        {
            var enrollment = _dbContext.Enrollments
                .Where(e => e.Semester == 1)
                .FirstOrDefault(e => e.IdStudy == studies.IdStudy);
            if (enrollment != null) return enrollment;
            var newEnrollment = new Enrollment
            {
                Semester = 1,
                IdStudy = studies.IdStudy,
                StartDate = DateTime.Now
            };
            _dbContext.Enrollments.Add(newEnrollment);
            return newEnrollment;
        }
    }
}