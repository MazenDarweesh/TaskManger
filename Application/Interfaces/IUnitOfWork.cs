
using Application.Models;
using Domain.Models;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TaskDomain> TaskRepository { get; }
    Task<PagedList<TaskDomain>> GetPagedTasksAsync(PaginationParams paginationParams);
    Task<int> SaveAsync();
}
