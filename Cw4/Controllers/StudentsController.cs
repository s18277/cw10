using Cw4.DAL;
using Cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private static IDbStudentService _dbService;

        public StudentsController(IDbStudentService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetEntries());
        }

        [HttpGet("{idStudent}")]
        public IActionResult GetStudent([FromRoute] string idStudent)
        {
            var student = _dbService.GetStartedStudies(idStudent);
            if (student == null) return NotFound($"Nie odnaleziono studenta o id: {idStudent}!");
            return Ok(student);
        }

        [HttpPost]
        public IActionResult PostStudent([FromBody] Student student)
        {
            var affectedRows = _dbService.AddEntry(student);
            return Ok($"Zmodyfikowano {affectedRows} wiersz(y) w bazie danych.");
        }

        [HttpPut("{idStudent}")]
        public IActionResult PutStudent([FromRoute] string idStudent, [FromBody] Student newStudent)
        {
            newStudent.IndexNumber = idStudent;
            var affectedRows = _dbService.UpdateEntry(newStudent);
            return affectedRows == 0
                ? (IActionResult) NotFound($"Nie znaleziono studenta o numerze indeksu: {idStudent}!")
                : Ok($"Zmodyfikowano {affectedRows} wiersz(y) w bazie danych.");
        }

        [HttpDelete("{idStudent}")]
        public IActionResult DeleteStudent([FromRoute] string idStudent)
        {
            var affectedRows = _dbService.RemoveEntry(idStudent);
            return affectedRows == 0
                ? (IActionResult) NotFound($"Nie znaleziono studenta o numerze indeksu: {idStudent}!")
                : Ok($"Zmodyfikowano {affectedRows} wiersz(y) w bazie danych.");
        }
    }
}