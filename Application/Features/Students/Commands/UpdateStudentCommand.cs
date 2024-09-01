using MediatR;
using Application.DTOs;
using Application.IServices;

public class UpdateStudentCommand : IRequest
{
    public string Id { get; set; }
    public StudentDTO StudentDto { get; set; }
}


public class UpdateStudentCommandHandler(IStudentService studentService) : IRequestHandler<UpdateStudentCommand>
{
    private IStudentService _studentService = studentService;

    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        await _studentService.UpdateStudentAsync(request.Id, request.StudentDto);
    }
}
