using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;


namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<StudentDTO> _studentDtoValidator;
        private readonly IStringLocalizer<StudentService> _localizer;
        private readonly IMessagePublisher _messagePublisher;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger, IMapper mapper, IValidator<StudentDTO> studentDtoValidator, IStringLocalizer<StudentService> localizer, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _studentDtoValidator = studentDtoValidator;
            _localizer = localizer;
            _messagePublisher = messagePublisher;
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
            var students = await _unitOfWork.StudentRepository.GetPagedAsync(paginationParams, "Tasks");
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);

            _logger.LogInformation(_localizer[LocalizationKeys.StudentsRetrieved, students.Count]);
            return new PagedList<StudentDTO>(studentDtos, students.TotalCount, students.CurrentPage, students.PageSize);
        }

        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid, includeProperties: "Tasks");
            if(student == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.StudentNotFound, id]);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.StudentNotFound, id]);
            }
            _logger.LogInformation(_localizer[LocalizationKeys.StudentRetrieved, id]);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDto)
        {
            await ValidateStudentDtoAsync(studentDto);

            var student = _mapper.Map<Student>(studentDto);
            student.Id = Ulid.NewUlid(); // Generate Ulid here

            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Added a new student with id {StudentId}", student.Id);

            // Send email request to RabbitMQ
            var emailMessage = new EmailMessage
            {
                ToName = student.Name,
                ToEmail = student.Email,
                Subject = "Welcome To Our Website!!",
                Body = "Thank you for registering."
            };
            _messagePublisher.SendMessage(emailMessage);

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
