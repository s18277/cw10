﻿using System;
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
            _dbContext.Students.Remove(student);
            return _dbContext.SaveChanges();
        }
    }
}