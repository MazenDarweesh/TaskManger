using MediatR;
using Application.DTOs;
using Application.Features.Tasks.Task;
using Application.IServices;

namespace Application.Features.Tasks.Task
{
    public class UpdateTaskCommand : IRequest
    {
        public required string Id { get; set; }
        public required TaskDomainDTO TaskDto { get; set; }
    }
}

public class UpdateTaskCommandHandler(ITaskService taskService) : IRequestHandler<UpdateTaskCommand>
{
    private ITaskService _taskService = taskService;
    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskAsync(request.Id, request.TaskDto);
    }
}
