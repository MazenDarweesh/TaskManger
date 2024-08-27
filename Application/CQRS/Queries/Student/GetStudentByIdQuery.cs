using MediatR;
using Application.DTOs;

public class GetStudentByIdQuery : IRequest<StudentDTO>
{
    public string Id { get; set; }
}
