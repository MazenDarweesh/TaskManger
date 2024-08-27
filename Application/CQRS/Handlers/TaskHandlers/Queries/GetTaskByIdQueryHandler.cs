using MediatR;
using Application.DTOs;
using Application.IServices;
using System.Threading;
using System.Threading.Tasks;

public class GetTaskByIdQueryHandler : BaseQueryTaskHandler, IRequestHandler<GetTaskByIdQuery, TaskDomainDTO>
{
    public GetTaskByIdQueryHandler(ITaskService taskService) : base(taskService) { }

    public async Task<TaskDomainDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTaskByIdAsync(request.Id);
    }
}
