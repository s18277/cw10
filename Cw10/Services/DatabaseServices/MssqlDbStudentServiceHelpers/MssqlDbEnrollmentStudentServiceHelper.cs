using System;
using System.Data;
using System.Data.SqlClient;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cw10.DTOs.ResultContainers;
using Cw10.Exceptions;
using Cw10.Models.StudentsDatabase;

namespace Cw10.Services.DatabaseServices.MssqlDbStudentServiceHelpers
{
    internal class MssqlDbEnrollmentStudentServiceHelper
    {
        private const string StudiesFilterNameQuery =
            "SELECT IdStudy, Name FROM Studies WHERE Name = @StudiesName";

        private const string StudentFilterIndexNumberQuery =
            "SELECT IndexNumber FROM Student WHERE IndexNumber = @IndexNumber";

        private const string FirstSemesterEnrollmentForStudiesQuery =
            "SELECT IdEnrollment, Semester, Enrollment.IdStudy, StartDate FROM Enrollment " +
            "INNER JOIN Studies on Studies.IdStudy = Enrollment.IdStudy " +
            "WHERE Studies.Name = @StudiesName AND Semester = 1";

        private const string SelectLastAddedEnrollment =
            "SELECT IdEnrollment, Semester, Enrollment.IdStudy, StartDate FROM Enrollment " +
            "INNER JOIN Studies on Studies.IdStudy = Enrollment.IdStudy " +
            "WHERE Studies.Name = @StudiesName AND Semester = 1 AND StartDate = @EnrollmentDate";

        private const string InsertFirstSemesterEnrollmentForStudiesQuery =
            "INSERT INTO Enrollment (Semester, IdStudy, StartDate) SELECT 1, IdStudy, @EnrollmentDate FROM Studies " +
            "WHERE Studies.Name = @StudiesName";

        private const string InsertStudentQuery =
            "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, SaltPasswordHash) " +
            "VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment, @SaltPasswordHash)";

        private readonly Func<string, string> _encryptFunction;

        private readonly EnrollStudentRequest _enrollRequest;
        private readonly SqlCommand _sqlCommand;
        private string _enrollmentDate;

        public MssqlDbEnrollmentStudentServiceHelper(SqlCommand sqlCommand, EnrollStudentRequest enrollRequest,
            Func<string, string> encryptFunction)
        {
            _sqlCommand = sqlCommand;
            _enrollRequest = enrollRequest;
            _encryptFunction = encryptFunction;
        }

        public EnrollmentResult Enroll()
        {
            _enrollmentDate = DateTime.Now.ToShortDateString();
            var studies = GetStudies();
            if (studies == null)
                return new EnrollmentResult {Error = $"Kierunek {_enrollRequest.Studies} nie istnieje!"};
            if (IndexNumberExistsInDatabase())
                return new EnrollmentResult {Error = $"Numer indeksu {_enrollRequest.IndexNumber} już istnieje!"};
            var enrollment = GetEnrollmentForNewStudent(studies);
            var newStudent = new Student
            {
                IndexNumber = _enrollRequest.IndexNumber,
                FirstName = _enrollRequest.FirstName,
                LastName = _enrollRequest.LastName,
                BirthDate = _enrollRequest.BirthDate,
                IdEnrollment = enrollment.IdEnrollment
            };
            InsertNewStudentIntoDb(newStudent);
            return new EnrollmentResult
            {
                Successful = true,
                Student = newStudent,
                Enrollment = enrollment
            };
        }

        private Study GetStudies()
        {
            _sqlCommand.CommandText = StudiesFilterNameQuery;
            _sqlCommand.Parameters.AddWithValue("StudiesName", _enrollRequest.Studies);
            using var sqlDataReader = _sqlCommand.ExecuteReader();
            return sqlDataReader.Read()
                ? new Study {IdStudy = (int) sqlDataReader["IdStudy"], Name = sqlDataReader["Name"].ToString()}
                : null;
        }

        private bool IndexNumberExistsInDatabase()
        {
            _sqlCommand.CommandText = StudentFilterIndexNumberQuery;
            _sqlCommand.Parameters.AddWithValue("IndexNumber", _enrollRequest.IndexNumber);
            using var sqlDataReader = _sqlCommand.ExecuteReader();
            return sqlDataReader.Read();
        }

        private EnrollmentDto GetEnrollmentForNewStudent(Study studies)
        {
            _sqlCommand.CommandText = FirstSemesterEnrollmentForStudiesQuery;
            using var sqlDataReader = _sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
                return SqlDataReaderToEnrollment(sqlDataReader, _enrollRequest.Studies);

            sqlDataReader.Close();
            return PrepareNewEnrollment(studies);
        }

        private EnrollmentDto PrepareNewEnrollment(Study studies)
        {
            _sqlCommand.CommandText = InsertFirstSemesterEnrollmentForStudiesQuery;
            _sqlCommand.Parameters.AddWithValue("EnrollmentDate", _enrollmentDate);
            if (_sqlCommand.ExecuteNonQuery() == 0)
                throw new SqlInsertException("Błąd podczas tworzenia nowego wpisu w tablicy \"Enrollment\"!");
            return InsertedEnrollment(studies.Name);
        }

        private EnrollmentDto InsertedEnrollment(string studiesName)
        {
            _sqlCommand.CommandText = SelectLastAddedEnrollment;
            using var sqlDataReader = _sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            return SqlDataReaderToEnrollment(sqlDataReader, studiesName);
        }

        private static EnrollmentDto SqlDataReaderToEnrollment(IDataRecord sqlDataReader, string studiesName)
        {
            return new EnrollmentDto
            {
                IdEnrollment = (int) sqlDataReader["IdEnrollment"],
                Semester = (int) sqlDataReader["Semester"],
                IdStudy = (int) sqlDataReader["IdStudy"],
                StartDate = DateTime.Parse(sqlDataReader["StartDate"].ToString()!),
                Name = studiesName
            };
        }

        private void InsertNewStudentIntoDb(Student newStudent)
        {
            _sqlCommand.CommandText = InsertStudentQuery;
            _sqlCommand.Parameters.AddWithValue("FirstName", _enrollRequest.FirstName);
            _sqlCommand.Parameters.AddWithValue("LastName", _enrollRequest.LastName);
            _sqlCommand.Parameters.AddWithValue("BirthDate", _enrollRequest.BirthDate);
            _sqlCommand.Parameters.AddWithValue("SaltPasswordHash", _encryptFunction(_enrollRequest.Password));
            if (!_sqlCommand.Parameters.Contains("IdEnrollment"))
                _sqlCommand.Parameters.AddWithValue("IdEnrollment", newStudent.IdEnrollment);
            if (_sqlCommand.ExecuteNonQuery() == 0)
                throw new SqlInsertException("Błąd podczas tworzenia nowego wpisu w tablicy \"Students\"!");
        }
    }
}