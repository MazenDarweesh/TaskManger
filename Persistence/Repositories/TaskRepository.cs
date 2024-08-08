using Application.Interfaces;
using Domain.Models;

namespace Persistence.Repositories
{
    public class TaskRepository : GenericRepository<TaskDomain>, IGenericRepository<TaskDomain>
    {
        public TaskRepository(TaskContext context) : base(context) { }
        public override async Task<TaskDomain> AddAsync(TaskDomain entity)
        {
            entity.Id = Ulid.NewUlid();
            await base.AddAsync(entity);
            return entity;
        }
    }
}
