using MediatR;
using Application.DTOs;
using Application.IServices;

public class CreateTaskCommand : IRequest<TaskDomainDTO>
{
    public TaskDomainDTO TaskDto { get; set; }
}

public class CreateTaskCommandHandler(ITaskService taskService) : IRequestHandler<CreateTaskCommand, TaskDomainDTO>
{
    private readonly ITaskService _taskService;

    public async Task<TaskDomainDTO> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        return await _taskService.AddTaskAsync(request.TaskDto);
    }
}