using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StudentService> _localizer;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger, IMapper mapper, IStringLocalizer<StudentService> localizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PagedList<StudentDTO>> GetAllStudentsAsync(PaginationParams paginationParams)
        {
            var students = await _unitOfWork.StudentRepository.GetPagedAsync(paginationParams, "Tasks");
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);
            var test = _localizer[LocalizationKeys.Test];
            Console.WriteLine(test);
            _logger.LogInformation(_localizer["StudentRetrieved"]);
            return new PagedList<StudentDTO>(studentDtos, students.TotalCount, students.CurrentPage, students.PageSize);
        }

        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid(), includeProperties: "Tasks");
            if (student == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.StudentNotFound, id].Value);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id].Value);
            }
            _logger.LogInformation(_localizer[LocalizationKeys.StudentRetrieved, id].Value);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            student.Id = Ulid.NewUlid(); // Generate Ulid here

            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentAdded, student.Id].Value);

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task UpdateStudentAsync(string id, StudentDTO studentDto)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid());
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id].Value);
            }
            _mapper.Map(studentDto, student);

            _unitOfWork.StudentRepository.UpdateAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentUpdated, student.Id].Value);
        }

        public async Task DeleteStudentAsync(string id)
        {
           
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid());

            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id].Value);
            }
            await _unitOfWork.StudentRepository.DeleteAsync(id.ConvertToUlid());
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentDeleted, student.Id].Value);
        }
    }
}
