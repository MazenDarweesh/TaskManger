using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.DTOs;

namespace Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(TaskContext context) : base(context)
        {
        }

        public async Task<PagedList<Student>> GetPagedAsync(PaginationDTO paginationParams, params string[] includeProperties)
        {
            IQueryable<Student> query = _context.Set<Student>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var count = await query.CountAsync();
            var items = await query
                .OrderBy(student => student.Id) // Ensure a consistent order
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return new PagedList<Student>(items, count, paginationParams.PageNumber, paginationParams.PageSize);
        }
        public async Task<Student> GetByIdAsync(Ulid id, string includeProperties = "")
        {
            IQueryable<Student> query = _context.Set<Student>();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public override async Task<Student> AddAsync(Student entity)
        {
            entity.Id = Ulid.NewUlid();

            await base.AddAsync(entity);
            return entity;
        }
        
    }
}
