﻿using Application.DTOs;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IStudentService
    {
        Task<PagedList<StudentDTO>> GetAllStudentsAsync(PaginationParams paginationParams);
        Task<StudentDTO> GetStudentByIdAsync(string id);
        Task<StudentDTO> AddStudentAsync(StudentDTO studentDto);
        Task UpdateStudentAsync(string id, StudentDTO studentDto);
        Task DeleteStudentAsync(string id);
    }
}
