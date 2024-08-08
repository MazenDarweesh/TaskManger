using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Models;
using Newtonsoft.Json;
using Application.IServices;

namespace TaskManagementSolution.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : BaseController
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<TaskDomainDTO>>> GetTasks([FromQuery] PaginationParams paginationParams)
    {
        var tasks = await _taskService.GetTasksAsync(paginationParams);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new
        {
            tasks.CurrentPage,
            tasks.TotalPages,
            tasks.PageSize,
            tasks.TotalCount
        }));
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDomainDTO>> GetTask(string id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
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

        var createdTask = await _taskService.AddTaskAsync(taskDto);

        return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(string id, TaskDomainDTO taskDto)
    {
        if (!Ulid.TryParse(id, out var ulid))
        {
            return BadRequest("Invalid ID format.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _taskService.UpdateTaskAsync(id,taskDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(string id)
    {
        if (!Ulid.TryParse(id, out var ulid))
        {
            return BadRequest("Invalid ID format.");
        }

        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }
}
