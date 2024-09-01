using MediatR;
using Application.DTOs;
using Application.IServices;

public class GetStudentByIdQuery : IRequest<StudentDTO>
{
    public string Id { get; set; }
}
public class GetStudentByIdQueryHandler(IStudentService studentService) : IRequestHandler<GetStudentByIdQuery, StudentDTO>
{
    private readonly IStudentService _studentService;
    public async Task<StudentDTO> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _studentService.GetStudentByIdAsync(request.Id);
    }
}
