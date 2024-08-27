using Application.IServices;
using MediatR;

public class DeleteStudentCommandHandler : BaseCommandStudentHandler, IRequestHandler<DeleteStudentCommand>
{
    public DeleteStudentCommandHandler(IStudentService studentService) : base(studentService) { }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        await _studentService.DeleteStudentAsync(request.Id);
    }
}
