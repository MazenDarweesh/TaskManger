using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<TaskDomainDTO> _taskDtoValidator;

        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger, IMapper mapper, IValidator<TaskDomainDTO> taskDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _taskDtoValidator = taskDtoValidator;
        }
        private async Task ValidateTaskDtoAsync(TaskDomainDTO taskDto)
        {
            var validationResult = await _taskDtoValidator.ValidateAsync(taskDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for task DTO: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
        }

        public async Task<PagedList<TaskDomainDTO>> GetTasksAsync(PaginationParams paginationParams)
        {
            var tasks = await _unitOfWork.TaskRepository.GetPagedAsync(paginationParams);
            _logger.LogInformation("Retrieved {Count} tasks", tasks.Count);

            var taskDtos = _mapper.Map<List<TaskDomainDTO>>(tasks);
            return new PagedList<TaskDomainDTO>(taskDtos, tasks.TotalCount, tasks.CurrentPage, tasks.PageSize);
        }

        public async Task<TaskDomainDTO> GetTaskByIdAsync(string id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());
            if (task == null)
            {
                _logger.LogWarning("Task with id {TaskId} not found", id);
                throw new KeyNotFoundException($"Task with id {id} not found");
            }

            _logger.LogInformation("Retrieved task with id {TaskId}", id);
            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task<TaskDomainDTO> AddTaskAsync(TaskDomainDTO taskDto)
        {
            await ValidateTaskDtoAsync(taskDto);

            var task = _mapper.Map<TaskDomain>(taskDto);
            task.Id = Ulid.NewUlid();

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Created a new task with id {TaskId}", task.Id);

            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task UpdateTaskAsync(string id, TaskDomainDTO taskDto)
        {
            await ValidateTaskDtoAsync(taskDto);

            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());
            if (task == null)
            {
                _logger.LogWarning("Task with id {TaskId} not found", id);
                throw new KeyNotFoundException($"Task with id {id} not found");
            }

            _mapper.Map(taskDto, task);

            _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Updated task with id {TaskId}", task.Id);
        }

        public async Task DeleteTaskAsync(string id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());

            if (task == null)
            {
                _logger.LogWarning("Task with id {TaskId} not found", id);
                throw new KeyNotFoundException($"Task with id {id} not found");
            }

            await _unitOfWork.TaskRepository.DeleteAsync(id.ConvertToUlid());
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted task with id {TaskId}", id);
        }

    }
}