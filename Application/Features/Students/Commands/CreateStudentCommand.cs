using MediatR;
using Application.DTOs;
using Application.IServices;
using Application.Services;

public class CreateStudentCommand : IRequest<StudentDTO>
{
    public StudentDTO StudentDto { get; set; }
}


public class CreateStudentCommandHandler(IStudentService studentService) : IRequestHandler<CreateStudentCommand, StudentDTO>
{
    private IStudentService _studentService = studentService;

    public async Task<StudentDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.AddStudentAsync(request.StudentDto);
    }
}
//A CancellationToken is a struct in .NET that is used to propagate notifications that operations should be canceled.
//    It is commonly used in asynchronous programming to provide a way to cancel ongoing tasks, 
// 