using MediatR;
using Application.DTOs;

public class DeleteTaskCommand : IRequest
{
    public string Id { get; set; }
}
