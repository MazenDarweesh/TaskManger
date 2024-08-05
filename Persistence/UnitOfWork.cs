using Application.Interfaces;
using Persistence.Repositories;
using Domain.Models;
using Application.Models;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskContext _context;
        private IGenericRepository<TaskDomain> _taskRepository;

        public UnitOfWork(TaskContext context)
        {
            _context = context;
        }

        public IGenericRepository<TaskDomain> TaskRepository
        {
            get
            {
                return _taskRepository ??= new TaskRepository(_context);
            }
        }
        public async Task<PagedList<TaskDomain>> GetPagedTasksAsync(PaginationParams paginationParams)
        {
            return await TaskRepository.GetPagedAsync(paginationParams);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
