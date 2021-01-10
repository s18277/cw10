using Cw10.DTOs.Requests;
using Cw10.Services.DatabaseServices;
using Microsoft.AspNetCore.Mvc;

namespace Cw10.Controllers
{
    [ApiController]
    [Route("api/students")]
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
            return Ok(_dbService.GetAllStudents());
        }

        [HttpGet("{idStudent}")]
        public IActionResult GetStudent([FromRoute] string idStudent)
        {
            var student = _dbService.GetStudent(idStudent);
            if (student == null) return NotFound($"Nie odnaleziono studenta o id: {idStudent}!");
            return Ok(student);
        }

        [HttpPut("{idStudent}")]
        public IActionResult UpdateStudent([FromRoute] string idStudent,
            [FromBody] UpdateStudentRequest updateStudentRequest)
        {
            var modifiedLines = _dbService.UpdateStudent(idStudent, updateStudentRequest);
            if (modifiedLines != 0) return Ok();
            return BadRequest("Nie zmodyfikowano żadnego wiersza!");
        }

        [HttpDelete("{idStudent}")]
        public IActionResult DeleteStudent([FromRoute] string idStudent)
        {
            var modifiedLines = _dbService.DeleteStudent(idStudent);
            if (modifiedLines != 0) return Ok();
            return BadRequest("Nie usunięto żadnego studenta!");
        }
    }
}