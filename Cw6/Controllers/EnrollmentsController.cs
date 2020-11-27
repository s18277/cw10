using Cw6.DTOs.Requests;
using Cw6.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw6.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbStudentService _dbService;

        public EnrollmentsController(IDbStudentService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult Enroll(EnrollStudentRequest newStudentRequest)
        {
            var enrollmentResult = _dbService.EnrollStudent(newStudentRequest);
            if (enrollmentResult.Successful)
                return Created($"/api/students/{newStudentRequest.IndexNumber}", enrollmentResult.Enrollment);
            return BadRequest(enrollmentResult.Error);
        }
    }
}