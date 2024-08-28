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
//A CancellationToken is a struct in .NET that is used to propagate notifications that operations should be canceled.
//    It is commonly used in asynchronous programming to provide a way to cancel ongoing tasks, 
//    such as long-running operations or I/O-bound tasks.