using MediatR;
using Application.IServices;
using Application.Commands.Task;
public class UpdateTaskCommandHandler : BaseCommandTaskHandler, IRequestHandler<UpdateTaskCommand>
{
    public UpdateTaskCommandHandler(ITaskService taskService) : base(taskService) {}

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskAsync(request.Id, request.TaskDto);
    }
}
