using MediatR;
using Application.DTOs;
using Application.IServices;
using Application;

public class GetTasksQuery : IRequest<PagedList<TaskDomainDTO>>
{
    public PaginationDTO PaginationParams { get; set; }
}

public class GetTasksQueryHandler(ITaskService taskService) : IRequestHandler<GetTasksQuery, PagedList<TaskDomainDTO>>
{
    private ITaskService _taskService = taskService;
    public async Task<PagedList<TaskDomainDTO>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTasksAsync(request.PaginationParams);
    }
}
