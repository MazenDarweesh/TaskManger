using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(TaskContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<Student> query = _context.Set<Student>();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }
        // here prop is the problem 
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
