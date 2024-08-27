using MediatR;
using Application.DTOs;
using Application.Models;
using Application.IServices;

public class GetTasksQueryHandler : BaseQueryTaskHandler, IRequestHandler<GetTasksQuery, PagedList<TaskDomainDTO>>
{
    public GetTasksQueryHandler(ITaskService taskService) : base(taskService) { }

    public async Task<PagedList<TaskDomainDTO>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTasksAsync(request.PaginationParams);
    }
}
