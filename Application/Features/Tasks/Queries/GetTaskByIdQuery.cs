using MediatR;
using Application.DTOs;
using Application.IServices;

public class GetTaskByIdQuery : IRequest<TaskDomainDTO>
{
    public string Id { get; set; }
}

public class GetTaskByIdQueryHandler(ITaskService taskService) : IRequestHandler<GetTaskByIdQuery, TaskDomainDTO>
{
    private ITaskService _taskService = taskService;
    public async Task<TaskDomainDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTaskByIdAsync(request.Id);
    }
}
