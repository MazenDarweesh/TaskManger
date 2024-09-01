using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<PagedList<Student>> GetPagedAsync(PaginationDTO paginationParams, params string[] includeProperties);
        Task<Student> GetByIdAsync(Ulid id, string includeProperties = "");
    }
}
