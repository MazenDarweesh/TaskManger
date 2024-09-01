using Application.IServices;
using MediatR;

public class DeleteStudentCommand : IRequest
{
    public string Id { get; set; }
}

public class DeleteStudentCommandHandler(IStudentService studentService) : IRequestHandler<DeleteStudentCommand>
{
    private IStudentService _studentService = studentService;

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        await _studentService.DeleteStudentAsync(request.Id);
    }
}
