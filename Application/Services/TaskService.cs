using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.IServices;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskService> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<TaskService> _localizer;

        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger, IMapper mapper, IStringLocalizer<TaskService> localizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PagedList<TaskDomainDTO>> GetTasksAsync(PaginationParams paginationParams)
        {
            var tasks = await _unitOfWork.TaskRepository.GetPagedAsync(paginationParams);
            _logger.LogInformation(_localizer["TaskRetrieved"]);

            var taskDtos = _mapper.Map<List<TaskDomainDTO>>(tasks);
            return new PagedList<TaskDomainDTO>(taskDtos, tasks.TotalCount, tasks.CurrentPage, tasks.PageSize);
        }

        public async Task<TaskDomainDTO> GetTaskByIdAsync(string id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());
            if (task == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.TaskNotFound, id]);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.TaskNotFound, id]);
            }

            _logger.LogInformation(_localizer[LocalizationKeys.TaskRetrieved, id]);
            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task<TaskDomainDTO> AddTaskAsync(TaskDomainDTO taskDto)
        {
            var task = _mapper.Map<TaskDomain>(taskDto);
            task.Id = Ulid.NewUlid();

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.TaskAdded, task.Id]);

            return _mapper.Map<TaskDomainDTO>(task);
        }

        public async Task UpdateTaskAsync(string id, TaskDomainDTO taskDto)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());
            if (task == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.TaskNotFound, id]);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.TaskNotFound, id]);
            }

            _mapper.Map(taskDto, task);

            _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.TaskUpdated, task.Id]);
        }

        public async Task DeleteTaskAsync(string id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id.ConvertToUlid());

            if (task == null)
            {
                _logger.LogWarning(_localizer[LocalizationKeys.TaskNotFound, id]);
                throw new KeyNotFoundException(_localizer[LocalizationKeys.TaskNotFound, id]);
            }

            await _unitOfWork.TaskRepository.DeleteAsync(id.ConvertToUlid());
            await _unitOfWork.SaveAsync();
            _logger.LogInformation(_localizer[LocalizationKeys.TaskDeleted, id]);
        }
    }
}