using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<StudentDTO> _studentDtoValidator;
        private readonly IStringLocalizer<StudentService> _localizer;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger, IMapper mapper, IValidator<StudentDTO> studentDtoValidator, IStringLocalizer<StudentService> localizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _studentDtoValidator = studentDtoValidator;
            _localizer = localizer;
        }
        private async Task ValidateStudentDtoAsync(StudentDTO studentDto)
        {
            var validationResult = await _studentDtoValidator.ValidateAsync(studentDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for student DTO: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
        }

        public async Task<PagedList<StudentDTO>> GetAllStudentsAsync(PaginationParams paginationParams)
        {
            var data = await _distributedCache.GetAsync("students");
            if (data != null)
            {
                var studentredis = JsonSerializer.Deserialize<List<StudentDTO>>(data);
                var studentDtos2 = _mapper.Map<List<StudentDTO>>(studentredis);

                _logger.LogInformation(_localizer[LocalizationKeys.StudentsRetrieved, studentredis.Count]);
                return new PagedList<StudentDTO>(studentDtos2, 0, 1, 2);

            }
            var students = await _unitOfWork.StudentRepository.GetPagedAsync(paginationParams, "Tasks");
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);

            _logger.LogInformation(_localizer[LocalizationKeys.StudentsRetrieved, students.Count]);
            var res = new PagedList<StudentDTO>(studentDtos, students.TotalCount, students.CurrentPage, students.PageSize);
            await _distributedCache.SetAsync("students", Encoding.Default.GetBytes(JsonSerializer.Serialize(res)));

            return res;
        }


        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            //var data = await _distributedCache.GetAsync(id);
            //if (data != null)
            //{
            //    var studentRedis = JsonSerializer.Deserialize<StudentDTO>(data);   
            //}
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid(), includeProperties: "Tasks");
            if (student == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.StudentNotFound, id]);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id]);
            }
            _logger.LogInformation(_localizer[LocalizationKeys.StudentRetrieved, id]);
            var res = _mapper.Map<StudentDTO>(student);
            //await _distributedCache.SetAsync(id, Encoding.Default.GetBytes(JsonSerializer.Serialize(res)));

            return res;


        }

        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDto)
        {
            await ValidateStudentDtoAsync(studentDto);

            var student = _mapper.Map<Student>(studentDto);
            student.Id = Ulid.NewUlid(); // Generate Ulid here

            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentAdded, student.Id]);

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task UpdateStudentAsync(string id, StudentDTO studentDto)
        {
            await ValidateStudentDtoAsync(studentDto);

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid());
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id]);
            }
            _mapper.Map(studentDto, student);

            _unitOfWork.StudentRepository.UpdateAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentUpdated, student.Id]);
        }

        public async Task DeleteStudentAsync(string id)
        {
           
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id.ConvertToUlid());

            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id]);
            }
            await _unitOfWork.StudentRepository.DeleteAsync(id.ConvertToUlid());
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.StudentDeleted, student.Id]);
        }
    }
}
