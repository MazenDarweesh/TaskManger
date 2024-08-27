using MediatR;
using Application.DTOs;
using Application.Models;

public class GetTasksQuery : IRequest<PagedList<TaskDomainDTO>>
{
    public PaginationParams PaginationParams { get; set; }
}
