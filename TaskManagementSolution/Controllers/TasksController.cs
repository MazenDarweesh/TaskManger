using Application.Interfaces;
using Application.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TaskManagementSolution.Controllers;

[Route("api/[controller]")]
public class TasksController(IUnitOfWork unitOfWork) : BaseController
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<ActionResult<PagedList<TaskDomain>>> GetTasks([FromQuery] PaginationParams paginationParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetPagedAsync(paginationParams);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new
        {
            tasks.CurrentPage,
            tasks.TotalPages,
            tasks.PageSize,
            tasks.TotalCount
        }));
        LogInformation("Retrieved all tasks");
        return Ok(tasks);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDomain>> GetTask(int id)
    {
        var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (task == null)
        {
            LogWarning("Task with id {TaskId} not found", id);
            return NotFound();
        }

        LogInformation("Retrieved task with id {TaskId}", id);
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDomain>> PostTask(TaskDomain task)
    {
        await _unitOfWork.TaskRepository.AddAsync(task);
        await _unitOfWork.SaveAsync();
        LogInformation("Created a new task with id {TaskId}", task.Id);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id, TaskDomain task)
    {
        if (id != task.Id)
        {
            LogWarning("Task id mismatch. Provided id: {TaskId}, Task id: {TaskObjectId}", id, task.Id);
            return BadRequest();
        }

        _unitOfWork.TaskRepository.UpdateAsync(task);
        await _unitOfWork.SaveAsync();
        LogInformation("Updated task with id {TaskId}", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await _unitOfWork.TaskRepository.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
        LogInformation("Deleted task with id {TaskId}", id);
        return NoContent();
    }
}

