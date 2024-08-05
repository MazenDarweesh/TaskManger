using Application.Interfaces;
using Domain.Models;

namespace Persistence.Repositories
{
    public class TaskRepository : GenericRepository<TaskDomain>, IGenericRepository<TaskDomain>
    {
        public TaskRepository(TaskContext context) : base(context) { }
    }
}
