using Application.IServices;
using MediatR;

public class UpdateStudentCommandHandler : BaseCommandStudentHandler, IRequestHandler<UpdateStudentCommand>
{
    public UpdateStudentCommandHandler(IStudentService studentService) : base(studentService) { }

    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        await _studentService.UpdateStudentAsync(request.Id, request.StudentDto);
    }
}
