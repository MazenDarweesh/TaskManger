using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _unitOfWork.StudentRepository.GetAllAsync(includeProperties: "Tasks");
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid, includeProperties: "Tasks");
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task AddStudentAsync(StudentDTO studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            student.Id = Ulid.NewUlid(); // Generate Ulid here
            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Added a new student with id {StudentId}", student.Id);
        }

        public async Task UpdateStudentAsync(string id, StudentDTO studentDto)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid);
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                return;
            }
            _mapper.Map(studentDto, student);
            _unitOfWork.StudentRepository.UpdateAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Updated student with id {StudentId}", student.Id);
            //var student = _mapper.Map<Student>(studentDto);
            //_unitOfWork.StudentRepository.UpdateAsync(student);
            //await _unitOfWork.SaveAsync();
            //_logger.LogInformation("Updated student with id {StudentId}", student.Id);
        }


        public async Task DeleteStudentAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid);
            if (student != null)
            {
                _unitOfWork.StudentRepository.DeleteAsync(ulid);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Deleted student with id {StudentId}", student.Id);
            }
        }

    }
}
