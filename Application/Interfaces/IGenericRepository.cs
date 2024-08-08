using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PagedList<T>> GetPagedAsync(PaginationParams paginationParams);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Ulid id);
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        Task DeleteAsync(Ulid id);

    }
}
