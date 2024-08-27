using MediatR;
using Application.IServices;
using Application.DTOs;
public class CreateStudentCommandHandler :BaseCommandStudentHandler, IRequestHandler<CreateStudentCommand, StudentDTO>
{
    public CreateStudentCommandHandler(IStudentService studentService) : base(studentService) {}

    public async Task<StudentDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.AddStudentAsync(request.StudentDto);
    }
}
