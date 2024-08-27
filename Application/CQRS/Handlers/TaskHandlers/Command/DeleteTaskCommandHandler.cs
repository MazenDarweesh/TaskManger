using MediatR;
using Application.IServices;
using System.Threading;
using System.Threading.Tasks;

public class DeleteTaskCommandHandler : BaseCommandTaskHandler, IRequestHandler<DeleteTaskCommand>
{
    public DeleteTaskCommandHandler(ITaskService taskService) : base(taskService) {}

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(request.Id);
    }
}
