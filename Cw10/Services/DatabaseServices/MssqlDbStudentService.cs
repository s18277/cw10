using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;
using Cw10.Services.DatabaseServices.MssqlDbStudentServiceHelpers;
using Cw10.Services.EncryptionServices;
using Microsoft.Extensions.Configuration;

namespace Cw10.Services.DatabaseServices
{
    public class MssqlDbStudentService : IDbStudentService
    {
        private readonly string _connectionString;
        private readonly IEncryptionService _encryptionService;

        public MssqlDbStudentService(IEncryptionService encryptionService, IConfiguration configuration)
        {
            _connectionString = configuration["DatabaseConnectionString"];
            _encryptionService = encryptionService;
        }

        public IEnumerable<GetAllStudentsStudentResponse> GetAllStudents()
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand {Connection = sqlConnection};
            sqlConnection.Open();
            return new MssqlDbGetStudentServiceHelper(sqlCommand).GetAllStudents().Select(student =>
                new GetAllStudentsStudentResponse
                {
                    IndexNumber = student.IndexNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    BirthDate = student.BirthDate,
                    IdEnrollment = student.IdEnrollment
                });
        }

        public GetSingleStudentResponse GetStudent(string indexNumber)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand {Connection = sqlConnection};
            sqlConnection.Open();
            return new MssqlDbGetStudentServiceHelper(sqlCommand).GetStudent(indexNumber);
        }

        public EnrollmentResult EnrollStudent(EnrollStudentRequest newStudent)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand {Connection = sqlConnection};
            sqlConnection.Open();
            sqlCommand.Transaction = sqlConnection.BeginTransaction();
            try
            {
                var enrollmentResult = new MssqlDbEnrollmentStudentServiceHelper(sqlCommand, newStudent,
                    _encryptionService.Encrypt).Enroll();
                if (enrollmentResult.Successful) sqlCommand.Transaction.Commit();
                else sqlCommand.Transaction.Rollback();
                return enrollmentResult;
            }
            catch (Exception exception)
            {
                sqlCommand.Transaction.Rollback();
                return new EnrollmentResult {Error = $"Napotkano wyjątek podczas dodawania studenta - {exception}!"};
            }
        }

        public EnrollmentDto PromoteStudents(PromoteStudentsRequest promoteStudentsRequest)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand("PromoteStudents", sqlConnection)
                {CommandType = CommandType.StoredProcedure};
            sqlCommand.Parameters.AddWithValue("@Studies", promoteStudentsRequest.Studies);
            sqlCommand.Parameters.AddWithValue("@Semester", promoteStudentsRequest.Semester);
            sqlConnection.Open();
            var sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read()) return null;
            return new EnrollmentDto
            {
                IdEnrollment = (int) sqlDataReader["IdEnrollment"],
                Semester = (int) sqlDataReader["Semester"],
                IdStudy = (int) sqlDataReader["IdStudy"],
                Name = sqlDataReader["Name"].ToString(),
                StartDate = DateTime.Parse(sqlDataReader["StartDate"].ToString()!)
            };
        }

        public SingleStudentAuthenticationData GetStudentsAuthenticationData(string indexNumber)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand {Connection = sqlConnection};
            sqlConnection.Open();
            return new MssqlDbGetStudentServiceHelper(sqlCommand).GetStudentsAuthenticationData(indexNumber);
        }

        public int UpdateRefreshToken(string username, string refreshToken)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "UPDATE Student SET RefreshToken = @RefreshToken WHERE IndexNumber = @Username"
            };
            sqlCommand.Parameters.AddWithValue("@RefreshToken", refreshToken);
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlConnection.Open();
            return sqlCommand.ExecuteNonQuery();
        }

        public int UpdateStudent(string indexNumber, UpdateStudentRequest updateStudentRequest)
        {
            throw new NotImplementedException();
        }

        public int DeleteStudent(string indexNumber)
        {
            throw new NotImplementedException();
        }
    }
}