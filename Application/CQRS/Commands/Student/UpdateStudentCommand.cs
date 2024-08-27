using MediatR;
using Application.DTOs;

public class UpdateStudentCommand : IRequest
{
    public string Id { get; set; }
    public StudentDTO StudentDto { get; set; }
}
