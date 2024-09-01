using Application.DTOs;

namespace Application.IServices
{
    public interface ITaskService
    {
        Task<PagedList<TaskDomainDTO>> GetTasksAsync(PaginationDTO paginationParams);
        Task<TaskDomainDTO> GetTaskByIdAsync(string id);
        Task<TaskDomainDTO> AddTaskAsync(TaskDomainDTO task);
        Task UpdateTaskAsync(string id, TaskDomainDTO task);
        Task DeleteTaskAsync(string id);
    }
}
