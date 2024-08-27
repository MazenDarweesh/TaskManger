using Application.IServices;
using Application.DTOs;
using MediatR;
public class GetStudentByIdQueryHandler : BaseQueryStudentHandler, IRequestHandler<GetStudentByIdQuery, StudentDTO>
{
    public GetStudentByIdQueryHandler(IStudentService studentService) : base(studentService) { }

    public async Task<StudentDTO> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _studentService.GetStudentByIdAsync(request.Id);
    }
}
