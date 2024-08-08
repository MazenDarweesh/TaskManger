using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskService> _logger;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        //public async Task<ValidationResult> ValidateTaskAsync(TaskDomainDTO taskDto)
        //{
        //    var validationResult = await _validator.ValidateAsync(taskDto);
        //    if (!validationResult.IsValid)
        //    {
        //        foreach (var error in validationResult.Errors)
        //        {
        //            _logger.LogWarning("Validation error on {PropertyName}: {ErrorMessage}", error.PropertyName, error.ErrorMessage);
        //        }
        //    }
        //    return validationResult;
        //}
        public async Task<PagedList<TaskDomainDTO>> GetTasksAsync(PaginationParams paginationParams)
        {
            var tasks = await _unitOfWork.TaskRepository.GetPagedAsync(paginationParams);
            _logger.LogInformation("Retrieved {Count} tasks", tasks.Count);

            var taskDtos = _mapper.Map<List<TaskDomainDTO>>(tasks);
            return new PagedList<TaskDomainDTO>(taskDtos, tasks.TotalCount, tasks.CurrentPage, tasks.PageSize);
        }

        public async Task<TaskDomainDTO> GetTaskByIdAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(ulid);
            if (task == null)
            {
                _logger.LogWarning("Task with id {TaskId} not found", id);
                return null;
            }

            _logger.LogInformation("Retrieved task with id {TaskId}", id);
            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task<TaskDomainDTO> AddTaskAsync(TaskDomainDTO taskDto)
        {
            var task = _mapper.Map<TaskDomain>(taskDto);
            task.Id = Ulid.NewUlid(); 

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Created a new task with id {TaskId}", task.Id);

            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task UpdateTaskAsync(string id,TaskDomainDTO taskDto)
        {
            var ulid = Ulid.Parse(id);
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(ulid);
            if (task == null)
            {
                _logger.LogWarning("Task with id {TaskId} not found", id);
                return ;
            }

            _mapper.Map(taskDto, task);

            _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Updated task with id {TaskId}", task.Id);
        }

        public async Task DeleteTaskAsync(string id)
        {
            var ulid = Ulid.Parse(id);
            await _unitOfWork.TaskRepository.DeleteAsync(ulid);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted task with id {TaskId}", id);
        }
    }
}
