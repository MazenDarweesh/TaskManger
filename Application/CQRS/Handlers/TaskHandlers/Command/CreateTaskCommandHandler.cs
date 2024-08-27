using MediatR;
using Application.DTOs;
using Application.IServices;

namespace Application.Handlers.TaskHandlers.Command
{
    public class CreateTaskCommandHandler : BaseCommandTaskHandler, IRequestHandler<CreateTaskCommand, TaskDomainDTO>
    {
        public CreateTaskCommandHandler(ITaskService taskService) : base(taskService) { }

        public async Task<TaskDomainDTO> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            return await _taskService.AddTaskAsync(request.TaskDto);
        }
    }
}