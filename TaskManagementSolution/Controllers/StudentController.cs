using Application.DTOs;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Newtonsoft.Json;
using MediatR;
using Application.Validators;

namespace TaskManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<StudentDTO>>> GetStudents([FromQuery] PaginationParams? paginationParams)
        {
            var validPaginationParams = new PaginationValidator().Validate(paginationParams);
            
            if (!validPaginationParams.IsValid)
            {
                return BadRequest(validPaginationParams.Errors);
            }
            var query = new GetStudentsQuery { PaginationParams = paginationParams };
            var students = await _mediator.Send(query);
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
            var query = new GetStudentByIdQuery { Id = id };
            var student = await _mediator.Send(query);
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
            var command = new CreateStudentCommand { StudentDto = studentDto };
            var createdStudent = await _mediator.Send(command);
            // we will be using the ISender interface from MediatR to send the commands/queries to its registered handlers.
            // Alternatively, you can also use the IMediator interface, but the ISender interface is far more lightweight
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
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

            var command = new UpdateStudentCommand { Id = id, StudentDto = studentDto };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            if (!Ulid.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format.");
            }

            var command = new DeleteStudentCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
