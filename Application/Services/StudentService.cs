using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
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
        private readonly IMessagePublisher _messagePublisher;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<PagedList<StudentDTO>> GetAllStudentsAsync(PaginationParams paginationParams)
        {
            var students = await _unitOfWork.StudentRepository.GetPagedAsync(paginationParams, "Tasks");
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);
            return new PagedList<StudentDTO>(studentDtos, students.TotalCount, students.CurrentPage, students.PageSize);
        }

        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid, includeProperties: "Tasks");
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException($"Student with id {id} not found");
            }
            _logger.LogInformation("Retrieved student with id {StudentId}", id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDto)
        {
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
                Subject = "Welcome to the system",
                Body = "Thank you for registering."
            };
            _messagePublisher.SendMessage(emailMessage);

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task UpdateStudentAsync(string id, StudentDTO studentDto)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid);
            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException($"Student with id {id} not found");
            }
            _mapper.Map(studentDto, student);

            _unitOfWork.StudentRepository.UpdateAsync(student);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Updated student with id {StudentId}", student.Id);
        }

        public async Task DeleteStudentAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(ulid);

            if (student == null)
            {
                _logger.LogWarning("Student with id {StudentId} not found", id);
                throw new KeyNotFoundException($"Student with id {id} not found");
            }
            await _unitOfWork.StudentRepository.DeleteAsync(ulid);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted student with id {StudentId}", student.Id);
        }
    }
}
