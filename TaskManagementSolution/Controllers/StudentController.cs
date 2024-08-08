using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TaskManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(string id)
        {
            if (!Ulid.TryParse(id, out var ulid))
            {
                return BadRequest("Invalid ID format.");
            }

            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _studentService.AddStudentAsync(studentDto);
            return CreatedAtAction(nameof(GetStudent), new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, StudentDTO studentDto)
        {
            if (!Ulid.TryParse(id, out var ulid))
            {
                return BadRequest("Invalid ID format.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _studentService.UpdateStudentAsync(id, studentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            // The out var ulid part means that if the parsing is successful, the parsed Ulid value will be stored in the variable ulid.
            if (!Ulid.TryParse(id, out var ulid))
            {
                return BadRequest("Invalid ID format.");
            }
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
