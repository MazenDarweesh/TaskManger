using MediatR;
using Application.DTOs;

public class CreateStudentCommand : IRequest<StudentDTO>
{
    public StudentDTO StudentDto { get; set; }
}
