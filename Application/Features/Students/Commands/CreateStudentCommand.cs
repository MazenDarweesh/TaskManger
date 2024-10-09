using MediatR;
using Application.DTOs;
using Application.Services;
using Application.Interfaces.IServices;

public class CreateStudentCommand : IRequest<StudentDTO>
{
    public StudentDTO StudentDto { get; set; }
}
// بنظبط شكل الداتا اللي عايزين نشتغل عليها 

public class CreateStudentCommandHandler(IStudentService studentService) : IRequestHandler<CreateStudentCommand, StudentDTO>
{
    private IStudentService _studentService = studentService;

    public async Task<StudentDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.AddStudentAsync(request.StudentDto);
    }
}
// الهاندلر جواه السرفيس بتعمل اللوجيك وبترجع dto

//A CancellationToken is a struct in .NET that is used to propagate notifications that operations should be canceled.
//    It is commonly used in asynchronous programming to provide a way to cancel ongoing tasks, 
//all the handlers would implement the IRequestHandler<T, R> where T is the incoming request (which in our case would be the Query itself), and R would be the response, which is a list of products.
