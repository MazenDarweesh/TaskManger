using Application.DTOs;
using Application.Models;

namespace Application.IServices
{
    public interface ITaskService
    {
        Task<PagedList<TaskDomainDTO>> GetTasksAsync(PaginationParams paginationParams);
        Task<TaskDomainDTO> GetTaskByIdAsync(string id);
        Task<TaskDomainDTO> AddTaskAsync(TaskDomainDTO task);
        Task UpdateTaskAsync(string id, TaskDomainDTO task);
        Task DeleteTaskAsync(string id);
    }
}
