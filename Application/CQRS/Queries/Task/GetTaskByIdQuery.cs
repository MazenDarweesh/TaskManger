using MediatR;
using Application.DTOs;

public class GetTaskByIdQuery : IRequest<TaskDomainDTO>
{
    public string Id { get; set; }
}
