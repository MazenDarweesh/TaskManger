using MediatR;
using Application.IServices;
using System.Threading;
using System.Threading.Tasks;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskService _taskService;

    public DeleteTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(request.Id);
    }
}
