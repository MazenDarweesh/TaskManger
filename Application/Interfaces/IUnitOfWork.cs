using Application.DTOs;
using Domain.Models;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TaskDomain> TaskRepository { get; }
    IStudentRepository StudentRepository { get; }
    Task<PagedList<TaskDomain>> GetPagedTasksAsync(PaginationDTO paginationParams);
    Task<int> SaveAsync();
}
