using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO> GetStudentByIdAsync(string id);
        Task AddStudentAsync(StudentDTO studentDto);
        Task UpdateStudentAsync(string id, StudentDTO studentDto);
        Task DeleteStudentAsync(string id);
    }
}
