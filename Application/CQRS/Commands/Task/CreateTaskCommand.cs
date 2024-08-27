using MediatR;
using Application.DTOs;

public class CreateTaskCommand : IRequest<TaskDomainDTO>
{
    public TaskDomainDTO TaskDto { get; set; }
}