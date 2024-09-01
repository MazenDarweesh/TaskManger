using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Models;
using MediatR;
using Application.Features.Tasks.Task;

namespace TaskManagementSolution.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<TaskDomainDTO>>> GetTasks([FromQuery] PaginationParams paginationParams)
    {
        var query = new GetTasksQuery { PaginationParams = paginationParams };
        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDomainDTO>> GetTask(string id)
    {
        if (!Ulid.TryParse(id, out _))
        {
            return BadRequest("Invalid ID format.");
        }
        var query = new GetTaskByIdQuery { Id = id };
        var task = await _mediator.Send(query);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDomainDTO>> PostTask(TaskDomainDTO taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new CreateTaskCommand { TaskDto = taskDto };
        var createdTask = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(string id, TaskDomainDTO taskDto)
    {
        if (!Ulid.TryParse(id, out _))
        {
            return BadRequest("Invalid ID format.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateTaskCommand { Id = id, TaskDto = taskDto };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(string id)
    {
        if (!Ulid.TryParse(id, out _))
        {
            return BadRequest("Invalid ID format.");
        }

        var command = new DeleteTaskCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
