using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Newtonsoft.Json;

namespace TaskManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<StudentDTO>>> GetStudents([FromQuery] PaginationParams paginationParams)
        {
            var students = await _studentService.GetAllStudentsAsync(paginationParams);
            Response.Headers.Add("X-PaginationStudent", JsonConvert.SerializeObject(new
            {
                students.CurrentPage,
                students.TotalPages,
                students.PageSize,
                students.TotalCount
            }));
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(string id)
        {
            if (!Ulid.TryParse(id, out _))
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
            var createdStudent = await _studentService.AddStudentAsync(studentDto);

            return CreatedAtAction(nameof(GetStudent), new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, StudentDTO studentDto)
        {
            if (!Ulid.TryParse(id, out _))
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
            if (!Ulid.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format.");
            }

            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
