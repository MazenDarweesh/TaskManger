using MediatR;
using Application.IServices;
using Application.Commands.Task;
public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskService _taskService;

    public UpdateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskAsync(request.Id, request.TaskDto);
    }
}
