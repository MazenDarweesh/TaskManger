using MediatR;
using Application.IServices;

public class DeleteTaskCommand : IRequest
{
    public string Id { get; set; }
}

public class DeleteTaskCommandHandler(ITaskService taskService) : IRequestHandler<DeleteTaskCommand>
{
    private ITaskService _taskService = taskService;
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(request.Id);
    }
}
