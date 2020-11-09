using System.Linq;
using Cw4.DAL;
using Cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private static IDbService<Student> _dbService;

        public StudentsController(IDbService<Student> dbService)
        {
            _dbService = dbService;
        }

        // Przekazywanie danych poprzez segment URL
        [HttpGet("{idStudent}")]
        public IActionResult GetStudent([FromRoute] int idStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return NotFound($"Nie odnaleziono studenta o id: {idStudent}!");
            return Ok(student);
        }

        // Przekazywanie danych za pomocą Query-String
        [HttpGet]
        public IActionResult GetStudents([FromQuery] string orderBy)
        {
            var orderByToUse = orderBy ?? "IdStudent";
            var orderedEnumerable = _dbService.GetEntries();
            return orderByToUse.ToLower() switch
            {
                "firstname" => Ok(orderedEnumerable.OrderBy(student => student.FirstName)),
                "lastname" => Ok(orderedEnumerable.OrderBy(student => student.LastName)),
                "indexnumber" => Ok(orderedEnumerable.OrderBy(student => student.IndexNumber)),
                _ => Ok(orderedEnumerable.OrderBy(student => student.IdStudent))
            };
        }

        // Przykład metody POST tworzącej nowego studenta.
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IdStudent = _dbService.NextId();
            _dbService.AddEntry(student);
            return Ok($"Utworzono studenta: {student}.");
        }

        // Przykład metody PUT aktualizującej dane studenta.
        [HttpPut("{idStudent}")]
        public IActionResult PutStudent([FromRoute] int idStudent, [FromBody] Student newStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return CreateStudent(newStudent);
            student.FirstName = newStudent.FirstName;
            student.LastName = newStudent.LastName;
            return Ok($"Zmodyfikowano studenta: {student}.");
        }

        // Przykład metody delete usuwającej podanego studenta.
        [HttpDelete("{idStudent}")]
        public IActionResult DeleteStudent([FromRoute] int idStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return NotFound($"Nie odnaleziono studenta o id: {idStudent}!");
            _dbService.RemoveEntry(student);
            return Ok($"Usunięto studenta: {student}");
        }
    }
}