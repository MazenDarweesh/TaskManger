using MediatR;
using Application.DTOs;

namespace Application.Commands.Task
{
public class UpdateTaskCommand : IRequest
    {
        public required string Id { get; set; }
        public required TaskDomainDTO TaskDto { get; set; }
    }
}